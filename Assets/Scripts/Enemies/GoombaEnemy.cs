using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using System;

namespace Athena.Mario.Enemies
{
    public class GoombaEnemy : Enemy
    {
        [SerializeField] int moveDirection = 0;
        [SerializeField] float moveSpeed = 1f;
        [SerializeField] LayerMask DetectLayer;

        [SerializeField] private float scSideStartOffset = 0.2f;
        [SerializeField] private float scSideEndOffset = 0.2f;
        [SerializeField] private float scTopOffset = 0.5f;
        [SerializeField] private float scBottomOffset = -0.5f;
        [SerializeField] Animator animator;
        SpriteRenderer spriteRenderer;


        [SerializeField] private float tcTopStartOffset = 0.2f;
        [SerializeField] private float tcTopEndOffset = 0.2f;
        [SerializeField] private float tcLeftOffset = 0.5f;
        [SerializeField] private float tcRightOffset = -0.5f;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        

        private bool CheckForSideHit()
        {
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset * moveDirection, scTopOffset, 0f), new Vector2(moveDirection, 0f), scSideEndOffset, DetectLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset * moveDirection, scBottomOffset, 0f), new Vector2(moveDirection, 0f), scSideEndOffset, DetectLayer);

            RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset * -moveDirection, scTopOffset, 0f), new Vector2(moveDirection, 0f), scSideEndOffset, DetectLayer);
            RaycastHit2D hit4 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset * -moveDirection, scBottomOffset, 0f), new Vector2(moveDirection, 0f), scSideEndOffset, DetectLayer);

            if (hit1 || hit2 || hit3|| hit4)
            {
                Tuple<PlayerController, RaycastHit2D> plrHit = GetFirstPlayerFromRaycasts(hit1,hit2,hit3,hit4);
                if (plrHit == null)
                {
                    if (hit1 || hit2)
                    {
                        moveDirection *= -1;
                        return true;
                    }
                }
                else if (!plrHit.Item1.IsEffectActive(PowerEffects.EFFECT_CINV)|| !plrHit.Item1.IsEffectActive(PowerEffects.EFFECT_SINV))
                {
                    plrHit.Item1.GetHit();
                    return true;
                }
                else if(plrHit.Item1.IsEffectActive(PowerEffects.EFFECT_SINV))
                {
                    var hitDir = plrHit.Item2.collider.gameObject.transform.position.x <= transform.position.x;
                    StartCoroutine(GetPopped(hitDir));
                    return true;
                }
            }
            return false;
        }

        private Tuple<PlayerController, RaycastHit2D> GetFirstPlayerFromRaycasts(params RaycastHit2D[] hitList)
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

            private bool CheckForTopHit()
        {
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(tcLeftOffset, tcTopStartOffset, 0f), Vector3.up, tcTopEndOffset, DetectLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(tcRightOffset, tcTopStartOffset, 0f), Vector3.up, tcTopEndOffset, DetectLayer);
            
            if (hit1||hit2)
            {
                PlayerController plr = GetFirstPlayerFromRaycasts(hit1, hit2).Item1;
                if (plr != null)
                {
                    if (plr.IsEffectActive(PowerEffects.EFFECT_SINV))
                    { 
                        StartCoroutine(GetPopped()); 
                    }
                    else
                    {
                        plr.BounceOff();
                        StartCoroutine(GetSquashed());
                    }
                    return true;
                }
            }
            return false;
        }

        private IEnumerator GetSquashed()
        {
            animator.SetTrigger("Dead");
            isDead = true;
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(1f);
            Destroy(this);
        }

        private IEnumerator GetPopped(bool left=true)
        {
            isDead = true;
            
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
            spriteRenderer.flipY = true;
            float popForce = 15f; ;
            rb.AddForce(new Vector2(left?-0.75f:0.75f,1f) * popForce,ForceMode2D.Impulse);
            rb.gravityScale = 3;


            yield return new WaitUntil(()=>spriteRenderer.isVisible==false);

            Destroy(this);
        }
            private void OnDrawGizmos()
        {
            Vector3 scTopPos = transform.position + new Vector3(scSideStartOffset* moveDirection, scTopOffset, 0f);
            Vector3 scBotPos = transform.position + new Vector3(scSideStartOffset * moveDirection, scBottomOffset, 0f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(scTopPos, scTopPos + (new Vector3(moveDirection, 0f, 0f) * scSideEndOffset));
            Gizmos.DrawLine(scBotPos, scBotPos + (new Vector3(moveDirection, 0f, 0f) * scSideEndOffset));

            Vector3 tcLeftPos=transform.position + new Vector3(tcLeftOffset, tcTopStartOffset, 0f);
            Vector3 tcRightPos= transform.position + new Vector3(tcRightOffset, tcTopStartOffset, 0f);
            Gizmos.DrawLine(tcLeftPos, tcLeftPos + (Vector3.up * tcTopEndOffset));
            Gizmos.DrawLine(tcRightPos, tcRightPos + (Vector3.up * tcTopEndOffset));
        }

        private void FixedUpdate()
        {
            if (isActive&&!isDead)
            {

                var topHit= CheckForTopHit();
                if(!topHit)
                    CheckForSideHit();
                rb.velocity = moveDirection * moveSpeed * Vector2.right;
            }
        }
    }
}
