using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using System;
using System.Linq;
using Athena.Mario.Misc;
using FrostyScripts.Misc;
using UnityEngine.Serialization;

namespace Athena.Mario.Enemies
{
    public class KoopaEnemy : Enemy
    {//TODO : Separate Concerns
        public Direction MoveDirection { get => moveDirection;
            private set
            {
                prevMoveDirection = moveDirection;
                moveDirection = value;
                spriteRenderer.flipX = (value==Direction.RIGHT);
            }
        }

        private Direction prevMoveDirection = 0;
        public Direction moveDirection = 0;
        [SerializeField] float moveSpeed = 1f;
        [SerializeField] float slideSpeed = 2f;
       

        [SerializeField] private HitHandler hitHandler;
        [SerializeField] private List<HitData> currentHits;
        
        [SerializeField] private string squashedAnimName = "Squashed";
        private int squashedAnimHash;
        [SerializeField] private string slideAnimName = "Slide";
        private int slideAnimHash;
        [SerializeField] private string retAnimName = "Return";
        private int retAnimHash;

        [SerializeField] private float squashedReturnTime = 10f;
        [SerializeField] private float returnAnimLength = 2f;
        

        [SerializeField] private KoopaState state;
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
            if (hitHandler == null)
                hitHandler = GetComponent<HitHandler>();
            InitAnim();
            MoveDirection = Direction.RIGHT;
            State = KoopaState.KOOPA_NORMAL;
        }

        private KoopaState State
        {
            get => state;
            set
            {
                state = value;
                switch (value)
                {
                    case KoopaState.KOOPA_NORMAL:
                        rb.isKinematic = false;
                        rb.velocity = Vector2.zero;
                        if (MoveDirection == 0)
                        {

                            MoveDirection = (new System.Random().Next(1, 2) == 2 )? Direction.RIGHT : Direction.LEFT;
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
                            MoveDirection = (random.Next(2) == 1) ? Direction.RIGHT : Direction.LEFT;
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
        
        private bool CheckForHorizontalHit()
        {
            //TODO : Reimplement Hit Check

            var hList = currentHits.FindAll(x => x.hitSide == Direction.LEFT || x.hitSide == Direction.RIGHT);


            if (hList.Count == 0) return false;

            var plrHitData = hList.FirstOrDefault(x => x.hitObject.CompareTag("Player"));
            if (plrHitData != null) {
                var plrHit = plrHitData.hitObject.GetComponent<PlayerManager>();
                var hitSide = plrHitData.hitSide;
                if (plrHit.IsEffectActive(PowerEffects.EFFECT_SINV))
                {
                    var hitDir = plrHitData.hitSide == Direction.RIGHT;
                    GetPopped(hitDir);
                    return true;
                }
                switch (State)
                {
                    case KoopaState.KOOPA_SQUASHED:
                    {
                        var hitDir = hitSide == Direction.RIGHT;
                        MoveDirection = hitDir ? Direction.LEFT : Direction.RIGHT;
                        State = KoopaState.KOOPA_SLIDING;
                        break;
                    }
                    case KoopaState.KOOPA_SLIDING:
                        plrHit.GetHit();
                        break;
                    case KoopaState.KOOPA_NORMAL:
                        plrHit.GetHit();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return true;
            }

            if (State == KoopaState.KOOPA_SLIDING)
            {
                var popableHit = currentHits.FindAll(x => x.hitObject.GetComponent<IPopable>() != null);

                if (popableHit.Count != 0)
                {
                    foreach (var popable in popableHit)
                    {
                        var hitDir = popable.hitSide == Direction.RIGHT;
                        popable.hitObject.GetComponent<IPopable>().PopThis(hitDir);
                    }
                }
                else
                {
                    var terrainHits = currentHits.FindAll(x =>hitHandler.terrainLayer.Contains(x.hitObject.layer));
                    if (terrainHits.Count == 0) return false;
                    var hitSide = terrainHits.Count(x => x.hitSide == Direction.RIGHT) >
                            terrainHits.Count(x => x.hitSide == Direction.LEFT)?Direction.RIGHT:Direction.LEFT;
                    if (hitSide != moveDirection) return false;
                    MoveDirection = MoveDirection == Direction.RIGHT ? Direction.LEFT : Direction.RIGHT;
                    return true;
                }
            }
            else 
            {
                var terrainHits = currentHits.FindAll(x =>hitHandler.terrainLayer.Contains(x.hitObject.layer));
                if (terrainHits.Count == 0) return false;
                var hitSide = terrainHits.Count(x => x.hitSide == Direction.RIGHT) >
                              terrainHits.Count(x => x.hitSide == Direction.LEFT)?Direction.RIGHT:Direction.LEFT;
                if (hitSide != moveDirection) return false;
                MoveDirection = MoveDirection == Direction.RIGHT ? Direction.LEFT : Direction.RIGHT;
                return true;
            }

            return false;
        }
        
        private bool CheckForVerticalHit()
        { 
            //TODO : Reimplement Hit Check
            var hList = currentHits.FindAll(x => x.hitSide == Direction.TOP || x.hitSide == Direction.DOWN);

            if (hList.Count == 0) return false;

            var plrHitData = hList.FirstOrDefault(x => x.hitObject.CompareTag("Player"));
            if (plrHitData == null) return false;

            var plrHit = plrHitData.hitObject.GetComponent<PlayerManager>();
            var hitDir = plrHitData.hitSide;
            if (plrHit == null) return false;

            if (plrHit.IsEffectActive(PowerEffects.EFFECT_SINV))
            {
                StartCoroutine(PoppedDeath());
            }
            else if (plrHit.IsEffectActive(PowerEffects.EFFECT_CINV))
            {
                return false;
            }
            else
            {
                switch (hitDir)
                {
                    case Direction.TOP:
                        plrHit.BounceOff();
                        GetSquashed(false);
                        break;
                    default:
                        plrHit.GetHit();
                        return true;
                }
            }

            return true;
        }

        protected override void GetSquashed(bool hitRight)
        {

            switch (State)
            {
                case KoopaState.KOOPA_NORMAL:
                    State = KoopaState.KOOPA_SQUASHED;
                    break;
                case KoopaState.KOOPA_SQUASHED:
                    MoveDirection = hitRight ? Direction.RIGHT : Direction.LEFT;
                    State = KoopaState.KOOPA_SLIDING;
                    break;
                case KoopaState.KOOPA_SLIDING:
                    State = KoopaState.KOOPA_SQUASHED;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void FixedUpdate()
        {
            if (isActive && !isDead)
            {
                currentHits = hitHandler.GetHits();
                var topHit = CheckForVerticalHit();
                if (!topHit)
                    CheckForHorizontalHit();
                Move();
            }
        }

        private void Move()
        {
            switch (State)
            {
                case KoopaState.KOOPA_NORMAL:
                    rb.velocity = HitHandler.DirectionMap[MoveDirection] * moveSpeed ;
                    break;
                case KoopaState.KOOPA_SQUASHED:
                    rb.velocity = Vector2.zero;
                    break;
                case KoopaState.KOOPA_SLIDING:
                    rb.velocity = HitHandler.DirectionMap[MoveDirection] * slideSpeed;
                    break;
            }
        }
    }

    
}
