using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using System;

namespace Athena.Mario.Enemies
{
    
    public abstract class Enemy : MonoBehaviour, IPopable
    {
        [SerializeField] protected bool isActive;
        [SerializeField] protected Transform activationPoint;

        protected Rigidbody2D rb;
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;
        protected bool isDead = false;

        [SerializeField]protected string deadAnimName = "Dead";
        protected int deadAnimHash;

        public virtual void PopThis(bool dir)
        {
            GetPopped(dir);
        }
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = GetComponentInChildren<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponent<Animator>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
            deadAnimHash = Animator.StringToHash(deadAnimName);
        }

        protected Tuple<PlayerController, RaycastHit2D> GetFirstPlayerFromRaycasts(params RaycastHit2D[] hitList)
        {
            foreach (var hit in hitList)
            {
                if (hit)
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("Player Check");
                        return new Tuple<PlayerController, RaycastHit2D>(hit.collider.gameObject.GetComponent<PlayerController>(), hit);
                    }
                }
            }
            return null;
        }
        protected virtual void GetSquashed(bool hitRight)
        {
            Die(true, hitRight);
        }
        protected virtual void GetPopped(bool hitRight)
        {
            Die(false, hitRight);
        }

        protected virtual void Die(bool squashed,bool dir)
        {
            if(squashed)
                StartCoroutine(SquashDeath(dir));
            else
                StartCoroutine(PoppedDeath(dir));
        }

        protected virtual IEnumerator SquashDeath(bool dir = false)
        {
            animator.SetTrigger(deadAnimHash);
            isDead = true;
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(1f);
            Destroy(this);
        }
        protected virtual IEnumerator PoppedDeath(bool dir=false)
        {
            isDead = true;

            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
            spriteRenderer.flipY = true;
            float popForce = 15f;
            rb.AddForce(new Vector2(dir ? -0.75f : 0.75f, 1f) * popForce, ForceMode2D.Impulse);
            rb.gravityScale = 3;


            yield return new WaitUntil(() => spriteRenderer.isVisible == false);

            Destroy(this);
        }
    }
}
