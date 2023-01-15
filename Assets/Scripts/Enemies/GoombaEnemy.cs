using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using System;
using System.Linq;
using Athena.Mario.Misc;
using FrostyScripts.Misc;

namespace Athena.Mario.Enemies
{
    public class GoombaEnemy : Enemy
    {
        [SerializeField] private Direction moveDirection = 0;
        public Direction MoveDirection { get => moveDirection;
            private set => moveDirection = value;
        }
        
        [SerializeField] float moveSpeed = 1f;

        [SerializeField] private HitHandler hitHandler;
        [SerializeField] private List<HitData> currentHits;

        //TODO : Separate Concerns
        private bool CheckForHorizontalHit()
        {
            //TODO : Reimplement Hit Check
            var hList = currentHits.FindAll(x => x.hitSide == Direction.LEFT || x.hitSide == Direction.RIGHT);
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
                    var hitDir = plrHitData.hitSide == Direction.RIGHT;
                    StartCoroutine(PoppedDeath(hitDir));
                    return true;
                }

                return false;
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

        private bool CheckForVerticalHit()
        {
            //TODO : Reimplement Hit Check
            var hList = currentHits.FindAll(x => x.hitSide == Direction.TOP || x.hitSide == Direction.DOWN);
            if (hList.Count == 0) return false;
            
            var plrHitData = hList.FirstOrDefault(x => x.hitObject.CompareTag("Player"));
            if (plrHitData == null) return false;
            
            var plrHit = plrHitData.hitObject.GetComponent<PlayerManager>();
            var hitSide = plrHitData.hitSide;



            if (plrHit.IsEffectActive(PowerEffects.EFFECT_STAR))
            {
                StartCoroutine(PoppedDeath());
            }
            else if (plrHit.IsEffectActive(PowerEffects.EFFECT_COOLDOWN))
            {
                return false;
            }
            else
            {
                if (hitSide==Direction.TOP)
                {
                    plrHit.BounceOff();
                    
                    GetSquashed(true);
                }
                else
                {
                    plrHit.GetHit();
                    return true;
                }
            }

            return true;
        }

        protected override void Awake()
        {
            base.Awake();
            if (hitHandler == null)
                hitHandler = GetComponent<HitHandler>();
            MoveDirection = Direction.RIGHT;
        }

        private void Move()
        {
            rb.velocity = HitHandler.DirectionMap[MoveDirection] * moveSpeed;
        }

        private void FixedUpdate()
        {
            if (isActive&&!isDead)
            {
                currentHits = hitHandler.GetHits();
                var topHit= CheckForVerticalHit();
                if(!topHit)
                    CheckForHorizontalHit();
                Move();
            }
        }
    }
}
