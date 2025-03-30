using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using System;
using System.Linq;
using Athena.Mario.Misc;
using FrostyScripts.Misc;
using DG.Tweening;
using System.Data;

namespace Athena.Mario.Enemies
{
    public class PlantEnemy : Enemy
    {
        [SerializeField] private Direction moveDirection = 0;
        public Direction MoveDirection { get => moveDirection;
            private set => moveDirection = value;
        }

        [SerializeField] float moveSpeed = 0.3f;
        [SerializeField] float moveDistance = 1f;

        [SerializeField] float moveWaitTime = 3f;




        [SerializeField] private HitHandler hitHandler;
        [SerializeField] private List<HitData> currentHits;

        private Sequence moveSequenceAnim;

        [SerializeField] Sprite[] sequenceSprites;

        [SerializeField] float animSpeed = 0.2f;
        [SerializeField] float movementCooldownTime = 5f;
        [SerializeField] float playerDistance = 1f;

        bool isMoving = false;

        bool movementCooldown = false;
        Transform originTransform;

        Timer cooldownTimer;

        PlayerManager player;
        

        //TODO : Separate Concerns
        private bool CheckForHits()
        {
            //TODO : Reimplement Hit Check
            var hList = currentHits;
            if (hList.Count == 0) return false;
            
            var plrHitData = hList.FirstOrDefault(x => x.hitObject.CompareTag("Player"));
            
            if (plrHitData != null)
            { 
                var plrHit = plrHitData.hitObject.GetComponent<PlayerManager>();
                var hitSide = plrHitData.hitSide;
                if (!plrHit.IsEffectActive(PowerEffects.EFFECT_COOLDOWN) &&
                    !plrHit.IsEffectActive(PowerEffects.EFFECT_STAR))
                {
                    plrHit.GetHit();
                    return true;
                }

                if (plrHit.IsEffectActive(PowerEffects.EFFECT_STAR))
                {
                    StartCoroutine(PoppedDeath());
                    return true;
                }

            }
            return false;
            
        }

        protected override void Awake()
        {
            base.Awake();
            if (hitHandler == null)
                hitHandler = GetComponent<HitHandler>();
            originTransform = transform;
            player = FindObjectOfType<PlayerManager>();
            BiteAnimation();
        }

        private void FixedUpdate()
        {
            if (isActive&&!isDead)
            {
                currentHits = hitHandler.GetHits();
                CheckForHits();

                if (!isMoving && !movementCooldown && !IsPlayerClose())
                {

                    MoveAnimation();
                }

                cooldownTimer?.Tick(Time.deltaTime);
            }
        }

        bool IsPlayerClose()
        {
            if (player == null) return false;
            var playerPos = player.transform.position;
            var enemyPos = transform.position;
            var distance = Vector3.Distance(playerPos, enemyPos);
            return distance < playerDistance;
        }

        void MoveAnimation()
        {
            isMoving = true;


            // lerp in move direction for move distance over move speed
            moveSequenceAnim = DOTween.Sequence()
                .Append(transform.DOMove(originTransform.position + HitHandler.DirectionMap[MoveDirection] * moveDistance, moveSpeed))
                .AppendInterval(moveWaitTime)
                .Append(transform.DOMove(originTransform.position, moveSpeed))
                .SetLoops(0)
                .OnKill(() => OnMovementEnd())
                .Play();
        }

        void BiteAnimation()
        {
            DOTween.Sequence()
                .AppendCallback(() => spriteRenderer.sprite = sequenceSprites[1])
                .AppendInterval(animSpeed)
                .AppendCallback(() => spriteRenderer.sprite = sequenceSprites[0])
                .AppendInterval(animSpeed)
                .SetLoops(-1)
                .Play();
        }

        
void OnMovementEnd()
{
    movementCooldown = true;
    isMoving = false;
    cooldownTimer = new Timer(movementCooldownTime);
    cooldownTimer.OnTimerEnd += () => movementCooldown = false; 
}

        protected override IEnumerator PoppedDeath(bool hitDirection = false)
        {
            SetPoppedSprite();
            yield return StartCoroutine(base.PoppedDeath());
        }
        

        void SetPoppedSprite()
        {
            moveSequenceAnim.Kill();
            // spriteRenderer.sprite = poppedSprite;
        }
        
    
    }
}
