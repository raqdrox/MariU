using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using System;
using Athena.Mario.Misc;
using Random = UnityEngine.Random;

namespace Athena.Mario.Enemies
{
    public class KoopaEnemy : Enemy
    {
        [SerializeField] public int MoveDirection { get => moveDirection;
            private set
            {
                prevMoveDirection = moveDirection;
                moveDirection = value;
                spriteRenderer.flipX = (value==1);
            }
        }

        private int prevMoveDirection = 0;
        public int moveDirection = 0;
        [SerializeField] float moveSpeed = 1f;
        [SerializeField] float slideSpeed = 2f;
        [SerializeField] LayerMask DetectLayer;


        [SerializeField] private CollisionRaycastValues normalCollisionRaycastValues;

        [SerializeField] private string squashedAnimName = "Squashed";
        private int squashedAnimHash;
        [SerializeField] private string slideAnimName = "Slide";
        private int slideAnimHash;
        [SerializeField] private string retAnimName = "Return";
        private int retAnimHash;

        [SerializeField] private float squashedReturnTime = 10f;
        [SerializeField] private float returnAnimLength = 2f;
        

        [SerializeField] private KoopaState _state;
        private Coroutine squashedEnumerator;
        private void InitAnim()
        {
            squashedAnimHash = Animator.StringToHash(squashedAnimName);
            slideAnimHash = Animator.StringToHash(slideAnimName);
            retAnimHash = Animator.StringToHash(retAnimName);
        }

        protected override void Awake()
        {
            base.Awake();
            InitAnim();
            MoveDirection = MoveDirection;
            State = KoopaState.KOOPA_NORMAL;
        }

        private KoopaState State
        {
            get => _state;
            set
            {
                _state = value;
                switch (value)
                {
                    case KoopaState.KOOPA_NORMAL:
                        rb.isKinematic = false;
                        rb.velocity = Vector2.zero;
                        if (MoveDirection == 0)
                        {

                            MoveDirection = (new System.Random().Next(1, 2) == 2 )? 1 : -1;
                        }
                        break;
                    case KoopaState.KOOPA_SQUASHED:
                        MoveDirection = 0;
                        rb.velocity=Vector2.zero;
                        rb.isKinematic = true;
                        animator.SetTrigger(squashedAnimHash);
                        squashedEnumerator = StartCoroutine(SquashedEnumerator());
                        break;
                    case KoopaState.KOOPA_SLIDING:
                        rb.isKinematic = false;
                        StopCoroutine(squashedEnumerator);
                        squashedEnumerator = null;
                        if (MoveDirection==0)
                        {
                            var random = new System.Random();
                            MoveDirection = (random.Next(2) == 1) ? 1 : -1;
                        }
                        animator.SetTrigger(slideAnimHash);
                        break;
                    default:
                        break; 
                }
            }
        }

        private IEnumerator SquashedEnumerator()
        {
            yield return new WaitForSeconds(squashedReturnTime);

            animator.SetTrigger(retAnimHash);

            yield return new WaitForSeconds(returnAnimLength);

            State = KoopaState.KOOPA_NORMAL;
        }
        
        private CollisionRaycastValues GetRaycastValues()
        {
            switch (State)
            {
                case KoopaState.KOOPA_NORMAL:
                    return normalCollisionRaycastValues;
                case KoopaState.KOOPA_SQUASHED:
                    return normalCollisionRaycastValues;
                case KoopaState.KOOPA_SLIDING:
                    return normalCollisionRaycastValues;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CheckForHoriHit()
        {
            
            var mDir = MoveDirection;
            if (MoveDirection == 0)
                mDir = prevMoveDirection;
            else if (MoveDirection == 0 && prevMoveDirection == 0)
            {
                mDir = 1;
            }
            var rayValues = GetRaycastValues();
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(rayValues.scSideCenterOffset + (rayValues.scSideStartOffset * mDir), rayValues.scTopOffset, 0f), new Vector2(mDir, 0f), rayValues.scSideEndOffset, DetectLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(rayValues.scSideCenterOffset + (rayValues.scSideStartOffset * mDir), rayValues.scBottomOffset, 0f), new Vector2(mDir, 0f), rayValues.scSideEndOffset, DetectLayer);

            RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(rayValues.scSideCenterOffset + (rayValues.scSideStartOffset * -mDir), rayValues.scTopOffset, 0f), new Vector2(-mDir, 0f), rayValues.scSideEndOffset, DetectLayer);
            RaycastHit2D hit4 = Physics2D.Raycast(transform.position + new Vector3(rayValues.scSideCenterOffset + (rayValues.scSideStartOffset * -mDir), +rayValues.scBottomOffset, 0f), new Vector2(-mDir, 0f), rayValues.scSideEndOffset, DetectLayer);

            if (hit1 || hit2 || hit3 || hit4)
            {
                Tuple<PlayerController, RaycastHit2D> plrHit = GetFirstPlayerFromRaycasts(hit1, hit2, hit3, hit4);
                //REPLACE WITH ENTITY FINDING INSTEAD OF JUST PLAYER TO ALLOW ENEMIES OR OTHER ENTITIES TO GET HIT

                if (plrHit == null)
                {
                    if (State == KoopaState.KOOPA_SLIDING)
                    {
                        var popable = hit1.collider.GetComponent<IPopable>();//REPLACE WITH CHECK INVOLVING ALL FRONT COLLIDERS
                        if (popable!=null)
                        {
                            var hitDir= hit1.collider.gameObject.transform.position.x <= transform.position.x;
                            popable.PopThis(hitDir);
                        }
                    }
                    Debug.Log("Non Player Hit");
                    if (hit1 || hit2)
                    {
                        MoveDirection *= -1;
                        return true;
                    }

                    
                }
                else if (plrHit.Item1.IsEffectActive(PowerEffects.EFFECT_SINV))
                {
                    Debug.Log("Powered Player Hit");
                    var hitDir = plrHit.Item2.collider.gameObject.transform.position.x <= transform.position.x;
                    GetPopped(hitDir);
                    return true;
                }
                else
                {
                    if (State == KoopaState.KOOPA_SQUASHED)
                    {
                        Debug.Log("Hit a squashed koopa");
                        var hitDir = plrHit.Item2.collider.gameObject.transform.position.x <= transform.position.x;
                        Debug.Log(plrHit.Item2.collider.gameObject.transform.position.x);
                        Debug.Log(transform.position.x);
                        Debug.Log(hitDir);
                        MoveDirection = hitDir ? 1 : -1;
                        State = KoopaState.KOOPA_SLIDING;
                    }
                    else
                    {
                        Debug.Log("Player Hit");
                        plrHit.Item1.GetHit();
                    }
                    return true;
                }

            }
            return false;
        }

        private bool CheckForVertHit()
        {
            var rayValues = GetRaycastValues();
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(rayValues.tcLeftOffset,rayValues. tcTopCenterOffset+rayValues. tcTopStartOffset, 0f), Vector3.up, rayValues.tcTopEndOffset, DetectLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(rayValues.tcRightOffset,rayValues. tcTopCenterOffset + rayValues.tcTopStartOffset, 0f), Vector3.up, rayValues.tcTopEndOffset, DetectLayer);
            RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(rayValues.tcLeftOffset, rayValues.tcTopCenterOffset - rayValues.tcTopStartOffset, 0f), Vector3.down, rayValues.tcTopEndOffset, DetectLayer);
            RaycastHit2D hit4 = Physics2D.Raycast(transform.position + new Vector3(rayValues.tcRightOffset, rayValues. tcTopCenterOffset - rayValues.tcTopStartOffset, 0f), Vector3.down, rayValues.tcTopEndOffset, DetectLayer);

            if (hit1 || hit2 || hit3 || hit4)
            {
                Tuple<PlayerController, RaycastHit2D> plrHit = GetFirstPlayerFromRaycasts(hit1, hit2, hit3, hit4);
                if (plrHit != null)
                {
                    if (plrHit.Item1.IsEffectActive(PowerEffects.EFFECT_SINV))
                    {
                        StartCoroutine(PoppedDeath());
                    }
                    else
                    {
                        if (plrHit.Item2 == hit1 || plrHit.Item2 == hit2)
                        {
                            plrHit.Item1.BounceOff();
                            if (plrHit.Item2 == hit1)
                                GetSquashed(false);
                            else
                                GetSquashed(true);
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



        protected override void GetSquashed(bool hitRight)
        {

            switch (State)
            {
                case KoopaState.KOOPA_NORMAL:
                    State = KoopaState.KOOPA_SQUASHED;
                    break;
                case KoopaState.KOOPA_SQUASHED:
                    MoveDirection = hitRight ? 1 : -1;
                    State = KoopaState.KOOPA_SLIDING;
                    break;
                case KoopaState.KOOPA_SLIDING:
                    State = KoopaState.KOOPA_SQUASHED;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        private void OnDrawGizmos()
        {
            var rayValues = GetRaycastValues();
            if(rayValues==null)
                return;
            var mDir = MoveDirection;
            if (MoveDirection == 0)
                mDir = prevMoveDirection;
            Vector3 scTopPos = transform.position + new Vector3(rayValues.scSideCenterOffset + (rayValues.scSideStartOffset * mDir), rayValues.scTopOffset, 0f);
            Vector3 scBotPos = transform.position + new Vector3(rayValues.scSideCenterOffset + (rayValues.scSideStartOffset * mDir), rayValues.scBottomOffset, 0f);
            Vector3 scNegTopPos = transform.position + new Vector3(rayValues.scSideCenterOffset + (rayValues.scSideStartOffset * -mDir), rayValues.scTopOffset, 0f);
            Vector3 scNegBotPos = transform.position + new Vector3(rayValues.scSideCenterOffset + (rayValues.scSideStartOffset * -mDir), rayValues.scBottomOffset, 0f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(scTopPos, scTopPos + (new Vector3(mDir, 0f, 0f) * rayValues.scSideEndOffset));
            Gizmos.DrawLine(scBotPos, scBotPos + (new Vector3(mDir, 0f, 0f) * rayValues.scSideEndOffset));
            Gizmos.DrawLine(scNegTopPos, scNegTopPos + (new Vector3(-mDir, 0f, 0f) * rayValues.scSideEndOffset));
            Gizmos.DrawLine(scNegBotPos, scNegBotPos + (new Vector3(-mDir, 0f, 0f) * rayValues.scSideEndOffset));

            Vector3 tcLeftPos = transform.position + new Vector3(rayValues.tcLeftOffset, rayValues.tcTopCenterOffset + rayValues.tcTopStartOffset, 0f);
            Vector3 tcRightPos = transform.position + new Vector3(rayValues.tcRightOffset, rayValues.tcTopCenterOffset + rayValues.tcTopStartOffset, 0f);
            Vector3 tcNegLeftPos = transform.position + new Vector3(rayValues.tcLeftOffset, rayValues.tcTopCenterOffset - rayValues.tcTopStartOffset, 0f);
            Vector3 tcNegRightPos = transform.position + new Vector3(rayValues.tcRightOffset, rayValues.tcTopCenterOffset - rayValues.tcTopStartOffset, 0f);
            Gizmos.DrawLine(tcLeftPos, tcLeftPos + (Vector3.up * rayValues.tcTopEndOffset));
            Gizmos.DrawLine(tcRightPos, tcRightPos + (Vector3.up * rayValues.tcTopEndOffset));
            Gizmos.DrawLine(tcNegLeftPos, tcNegLeftPos + (Vector3.down * rayValues.tcTopEndOffset));
            Gizmos.DrawLine(tcNegRightPos, tcNegRightPos + (Vector3.down * rayValues.tcTopEndOffset));
        }
        private void FixedUpdate()
        {
            if (isActive && !isDead)
            {

                var topHit = CheckForVertHit();
                if (!topHit)
                    CheckForHoriHit();
                Move();
            }
        }

        private void Move()
        {
            switch (State)
            {
                case KoopaState.KOOPA_NORMAL:
                    rb.velocity = MoveDirection * moveSpeed * Vector2.right;
                    break;
                case KoopaState.KOOPA_SQUASHED:
                    rb.velocity = Vector2.zero;
                    break;
                case KoopaState.KOOPA_SLIDING:
                    rb.velocity = MoveDirection * slideSpeed * Vector2.right;
                    break;
            }
        }
    }
}
