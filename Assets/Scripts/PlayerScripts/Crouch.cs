using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarcellesUndercroft
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Crouch : Abilities
    {
        [SerializeField] protected LayerMask layers;

        private BoxCollider2D playerCollider;

        protected override void Initialisation()
        {
            base.Initialisation();
            playerCollider = GetComponent<BoxCollider2D>();
        }

        private void FixedUpdate()
        {
            CrouchHeld();
            Crouching();
        }

        protected virtual bool CrouchHeld()
        {
            if (Input.GetKey(KeyCode.C))
            {
                return true;
            }

            return false;
        }

        protected virtual void Crouching()
        {
            if (CrouchHeld() && character.IsGrounded)
            {
                anim.SetBool("Crouching", true);
                character.IsCrouching = true;
            }
            else
            {
                if (character.IsCrouching)
                {
                    if (CollisionCheck(Vector2.up, playerCollider.size.y * .25f, layers))
                    {
                        return;
                    }

                    character.IsCrouching = false;
                    anim.SetBool("Crouching", false);
                }
            }
        }
    }
}