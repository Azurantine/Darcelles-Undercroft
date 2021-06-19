using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarcellesUndercroft
{
    public class HorizontalMovement : Abilities
    {
        [SerializeField] protected float timeTilMaxSpeed;
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected float sprintMultiplier;
        [SerializeField] protected float crouchSpeedMultiplier;

        private float acceleration;
        private float currentSpeed;
        private float horizontalInput;
        private float runTime;

        protected override void Initialisation()
        {
            base.Initialisation();
        }

        protected virtual void Update()
        {
            MovementPressed();
            SprintingHeld();
        }

        protected virtual bool MovementPressed()
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool SprintingHeld()
        {
            if (Input.GetKey(KeyCode.LeftShift))
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
            Movement();
        }

        protected virtual void Movement()
        {
            if (MovementPressed())
            {
                anim.SetBool("Moving", true);
                acceleration = maxSpeed / timeTilMaxSpeed;
                runTime += Time.deltaTime;
                currentSpeed = horizontalInput * acceleration * runTime;
                CheckDirection();
            }
            else
            {
                anim.SetBool("Moving", false);
                acceleration = runTime = currentSpeed = 0;
            }

            SpeedMultiplier();
            anim.SetFloat("CurrentSpeed", currentSpeed);
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }

        protected virtual void CheckDirection()
        {
            if (currentSpeed > 0)
            {
                if (character.IsFacingLeft)
                {
                    character.IsFacingLeft = false;
                    Flip();
                }

                if (currentSpeed > maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
            }

            if (currentSpeed < 0)
            {
                if (!character.IsFacingLeft)
                {
                    character.IsFacingLeft = true;
                    Flip();
                }

                if (currentSpeed < -maxSpeed)
                {
                    currentSpeed = -maxSpeed;
                }
            }
        }

        protected virtual void SpeedMultiplier()
        {
            if (SprintingHeld())
            {
                currentSpeed *= sprintMultiplier;
            }

            if (character.IsCrouching)
            {
                currentSpeed *= crouchSpeedMultiplier;
            }
        }
    }
}