using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarcellesUndercroft
{
    public class Jump : Abilities
    {
        [SerializeField] protected bool limitAirJumps;
        [SerializeField] protected int maxJumps;
        [SerializeField] protected float jumpForce;
        [SerializeField] protected float holdForce;
        [SerializeField] protected float buttonHoldTime;
        [SerializeField] protected float distanceToCollider;
        [SerializeField] protected float maxJumpSpeed;
        [SerializeField] protected float maxFallSpeed;
        [SerializeField] protected float acceptedFallSpeed;
        [SerializeField] protected float glideTime;
        [SerializeField] [Range(-2, 2)] protected float gravity;
        [SerializeField] protected LayerMask collisionLayer;

        private bool isJumping;
        private float jumpCountDown;
        private int numJumpsLeft;
        private float fallCountDown;

        protected override void Initialisation()
        {
            base.Initialisation();
            numJumpsLeft = maxJumps;
            jumpCountDown = buttonHoldTime;
            fallCountDown = glideTime;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            JumpPressed();
            JumpHeld();
        }

        protected virtual bool JumpPressed()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Ensure that the player actually jumped since we dont want to allow them to jump if they fell.
                if (!character.IsGrounded && numJumpsLeft == maxJumps)
                {
                    isJumping = false;
                    return false;
                }

                if (limitAirJumps && Falling(acceptedFallSpeed))
                {
                    isJumping = false;
                    return false;
                }

                numJumpsLeft--;

                if (numJumpsLeft >= 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    jumpCountDown = buttonHoldTime;
                    isJumping = true;
                    fallCountDown = glideTime;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool JumpHeld()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void FixedUpdate()
        {
            IsJumping();
            Gliding();
            GroundCheck();
        }

        protected virtual void IsJumping()
        {
            if (isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce);
                AdditionalAir();
            }

            if (rb.velocity.y > maxJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
            }
        }

        protected virtual void Gliding()
        {
            if (Falling(0) && JumpHeld())
            {
                fallCountDown -= Time.deltaTime;

                if (fallCountDown > 0 && rb.velocity.y > acceptedFallSpeed)
                {
                    anim.SetBool("Gliding", true);
                    FallSpeed(gravity);
                    return;
                }
            }

            anim.SetBool("Gliding", false);
        }

        protected virtual void AdditionalAir()
        {
            if (JumpHeld())
            {
                jumpCountDown -= Time.deltaTime;

                if (jumpCountDown <= 0)
                {
                    jumpCountDown = 0;
                    isJumping = false;
                }
                else
                {
                    rb.AddForce(Vector2.up * holdForce);
                }
            }
            else
            {
                isJumping = false;
            }
        }

        protected virtual void GroundCheck()
        {
            if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !isJumping)
            {
                anim.SetBool("Grounded", true);
                character.IsGrounded = true;
                numJumpsLeft = maxJumps;
                fallCountDown = glideTime;
            }
            else
            {
                anim.SetBool("Grounded", false);
                character.IsGrounded = false;
                if (Falling(0) && rb.velocity.y < maxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
                }
            }

            anim.SetFloat("VerticalSpeed", rb.velocity.y);
        }
    }
}