using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using System;

namespace Athena.Mario.Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected bool isActive;
        [SerializeField] protected Transform activationPoint;

        protected Rigidbody2D rb;
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;
        protected bool isDead = false;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

        }

        protected Tuple<PlayerController, RaycastHit2D> GetFirstPlayerFromRaycasts(params RaycastHit2D[] hitList)
        {
            foreach (var hit in hitList)
            {
                if (hit)
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        return new Tuple<PlayerController, RaycastHit2D>(hit.collider.gameObject.GetComponent<PlayerController>(), hit);
                    }
                }
            }
            return null;
        }
        protected virtual IEnumerator GetSquashed()
        {
            animator.SetTrigger("Dead");
            isDead = true;
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(1f);
            Destroy(this);
        }
        protected virtual IEnumerator GetPopped(bool dir=false)
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
