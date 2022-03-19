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



        [SerializeField] private float tcTopStartOffset = 0.2f;
        [SerializeField] private float tcTopEndOffset = 0.2f;
        [SerializeField] private float tcLeftOffset = 0.5f;
        [SerializeField] private float tcRightOffset = -0.5f;

        

        private bool CheckForHoriHit()
        {
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset * moveDirection, scTopOffset, 0f), new Vector2(moveDirection, 0f), scSideEndOffset, DetectLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset * moveDirection, scBottomOffset, 0f), new Vector2(moveDirection, 0f), scSideEndOffset, DetectLayer);

            RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset * -moveDirection, scTopOffset, 0f), new Vector2(-moveDirection, 0f), scSideEndOffset, DetectLayer);
            RaycastHit2D hit4 = Physics2D.Raycast(transform.position + new Vector3(scSideStartOffset * -moveDirection, scBottomOffset, 0f), new Vector2(-moveDirection, 0f), scSideEndOffset, DetectLayer);

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
                else if (!plrHit.Item1.IsEffectActive(PowerEffects.EFFECT_CINV)&& !plrHit.Item1.IsEffectActive(PowerEffects.EFFECT_SINV))
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

        private bool CheckForVertHit()
        {
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(tcLeftOffset, tcTopStartOffset, 0f), Vector3.up, tcTopEndOffset, DetectLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(tcRightOffset, tcTopStartOffset, 0f), Vector3.up, tcTopEndOffset, DetectLayer);
            RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(tcLeftOffset, -tcTopStartOffset, 0f), Vector3.down, tcTopEndOffset, DetectLayer);
            RaycastHit2D hit4 = Physics2D.Raycast(transform.position + new Vector3(tcRightOffset, -tcTopStartOffset, 0f), Vector3.down, tcTopEndOffset, DetectLayer);

            if (hit1 || hit2 || hit3 || hit4) 
            {
                Tuple<PlayerController, RaycastHit2D> plrHit = GetFirstPlayerFromRaycasts(hit1, hit2, hit3 , hit4);
                if (plrHit != null)
                {
                    if (plrHit.Item1.IsEffectActive(PowerEffects.EFFECT_SINV))
                    { 
                        StartCoroutine(GetPopped()); 
                    }
                    else
                    {
                        if (plrHit.Item2==hit1 || plrHit.Item2 == hit2)
                        {
                            plrHit.Item1.BounceOff();
                            StartCoroutine(GetSquashed());
                        }
                        else
                        {
                            plrHit.Item1.GetHit();
                            return true;
                        }
                    }
                    return true;
                }
            }
            return false;
        }


        private void OnDrawGizmos()
        {
            Vector3 scTopPos = transform.position + new Vector3(scSideStartOffset* moveDirection, scTopOffset, 0f);
            Vector3 scBotPos = transform.position + new Vector3(scSideStartOffset * moveDirection, scBottomOffset, 0f);
            Vector3 scNegTopPos = transform.position + new Vector3(scSideStartOffset * -moveDirection, scTopOffset, 0f);
            Vector3 scNegBotPos = transform.position + new Vector3(scSideStartOffset * -moveDirection, scBottomOffset, 0f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(scTopPos, scTopPos + (new Vector3(moveDirection, 0f, 0f) * scSideEndOffset));
            Gizmos.DrawLine(scBotPos, scBotPos + (new Vector3(moveDirection, 0f, 0f) * scSideEndOffset));
            Gizmos.DrawLine(scNegTopPos, scNegTopPos + (new Vector3(-moveDirection, 0f, 0f) * scSideEndOffset));
            Gizmos.DrawLine(scNegBotPos, scNegBotPos + (new Vector3(-moveDirection, 0f, 0f) * scSideEndOffset));

            Vector3 tcLeftPos=transform.position + new Vector3(tcLeftOffset, tcTopStartOffset, 0f);
            Vector3 tcRightPos= transform.position + new Vector3(tcRightOffset, tcTopStartOffset, 0f);
            Vector3 tcNegLeftPos = transform.position + new Vector3(tcLeftOffset, -tcTopStartOffset, 0f);
            Vector3 tcNegRightPos = transform.position + new Vector3(tcRightOffset, -tcTopStartOffset, 0f);
            Gizmos.DrawLine(tcLeftPos, tcLeftPos + (Vector3.up * tcTopEndOffset));
            Gizmos.DrawLine(tcRightPos, tcRightPos + (Vector3.up * tcTopEndOffset));
            Gizmos.DrawLine(tcNegLeftPos, tcNegLeftPos + (Vector3.down * tcTopEndOffset));
            Gizmos.DrawLine(tcNegRightPos, tcNegRightPos + (Vector3.down * tcTopEndOffset));
        }

        private void FixedUpdate()
        {
            if (isActive&&!isDead)
            {

                var topHit= CheckForVertHit();
                if(!topHit)
                    CheckForHoriHit();
                rb.velocity = moveDirection * moveSpeed * Vector2.right;
            }
        }
    }
}
