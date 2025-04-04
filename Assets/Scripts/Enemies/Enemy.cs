using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using System;

namespace Athena.Mario.Enemies
{
    
    public abstract class Enemy : MonoBehaviour, IPopable, IShootable
    {//TODO : Refactor Base Class
        [SerializeField] protected bool isActive;
        [SerializeField] protected Transform activationPoint;
        [SerializeField] protected bool isShootable = true;


        protected Rigidbody2D rb;
        protected SpriteRenderer spriteRenderer;
        protected bool isDead = false;

        [SerializeField]protected string deadAnimName = "Dead";

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
            isDead = true;
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
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

            Destroy(gameObject);
        }

        public virtual void GetShot(bool dir)
        {
            if (isShootable)
                GetPopped(dir);
        }
    }
}
