using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;

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


        [SerializeField] private float tcTopStartOffset = 0.2f;
        [SerializeField] private float tcTopEndOffset = 0.2f;
        [SerializeField] private float tcLeftOffset = 0.5f;
        [SerializeField] private float tcRightOffset = -0.5f;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }
        private void CheckForSideHit()
        {
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset, scTopOffset, 0f), new Vector2(moveDirection, 0f), scSideEndOffset, DetectLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset, scBottomOffset, 0f), new Vector2(moveDirection, 0f), scSideEndOffset, DetectLayer);
            if (hit1 || hit2)
            {
                PlayerController plr = null;
                if (hit1.collider.gameObject.GetComponent<PlayerController>())
                {
                    plr = hit1.collider.gameObject.GetComponent<PlayerController>();
                }
                else if (hit2.collider.gameObject.GetComponent<PlayerController>())
                {
                    plr = hit2.collider.gameObject.GetComponent<PlayerController>();
                }
                else
                {
                    moveDirection *= -1;
                    return;
                }
                plr.GetHit();
            }
        }


        private void CheckForTopHit()
        {
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(tcLeftOffset, tcTopStartOffset, 0f), Vector3.up, tcTopEndOffset, DetectLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(tcRightOffset, tcTopStartOffset, 0f), Vector3.up, tcTopEndOffset, DetectLayer);
            
            if (hit1||hit2)
            {
                
                PlayerController plr=null;
                if (hit1.collider.gameObject.GetComponent<PlayerController>())
                {
                    plr = hit1.collider.gameObject.GetComponent<PlayerController>();
                }
                else if (hit2.collider.gameObject.GetComponent<PlayerController>())
                {
                    plr = hit2.collider.gameObject.GetComponent<PlayerController>();
                }
                else
                {
                    return;
                }
                plr.BounceOff();
                StartCoroutine(GetSquashed());
            }
        }

        private IEnumerator GetSquashed()
        {
            animator.SetTrigger("Dead");
            isDead = true;
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(2f);
            Destroy(this);
        }

        private void OnDrawGizmos()
        {
            Vector3 scTopPos = transform.position + new Vector3(scSideStartOffset* moveDirection, scTopOffset, 0f);
            Vector3 scBotPos = transform.position + new Vector3(scSideStartOffset * moveDirection, scBottomOffset, 0f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(scTopPos, scBotPos + (new Vector3(moveDirection, 0f, 0f) * scSideEndOffset));
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
                CheckForSideHit();
                CheckForTopHit();
                rb.velocity = moveDirection * moveSpeed * Vector2.right;
            }
        }
    }
}
