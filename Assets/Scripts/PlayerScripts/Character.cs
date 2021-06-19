using UnityEngine;

namespace DarcellesUndercroft
{
    public class Character : MonoBehaviour
    {
        [HideInInspector] public bool IsFacingLeft;
        [HideInInspector] public bool IsGrounded;
        [HideInInspector] public bool IsCrouching;

        protected Collider2D col;
        protected Rigidbody2D rb;
        protected Animator anim;

        private Vector2 facingLeft;

        private void Start()
        {
            Initialisation();
        }

        protected virtual void Initialisation()
        {
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        protected virtual void Flip()
        {
            if (IsFacingLeft)
            {
                transform.localScale = facingLeft;
            }
            else
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }

        protected virtual bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);

            for (int i = 0; i < numHits; i++)
            {
                if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual bool Falling(float velocity)
        {
            if (!IsGrounded && rb.velocity.y < velocity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void FallSpeed(float speed)
        {
            rb.velocity = new Vector2(rb.velocity.x, (rb.velocity.y * speed));
        }
    }
}