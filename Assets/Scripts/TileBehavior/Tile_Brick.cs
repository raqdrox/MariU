using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using Athena.Mario.Entities;

namespace Athena.Mario.Tiles
{
    public class Tile_Brick : Entity
    {
        [SerializeField] float activationPower = 2f;
        [SerializeField] GameObject brickSprite;


        private ParticleSystem breakParticle;

        Collider2D[] colliders;
        Animator animator;

        public override EntityIdentifierEnum entityIdentifier => EntityIdentifierEnum.TILE_TERRAIN;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            colliders =GetComponents<Collider2D>();
            breakParticle = GetComponentInChildren<ParticleSystem>();
            
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.otherCollider.GetType() == typeof(PolygonCollider2D) && collision.gameObject.tag=="Player")
            {

                if (collision.relativeVelocity.y > activationPower)
                {
                    if (collision.gameObject.GetComponent<PlayerController>().CurrentPlayerState == PlayerStates.MARIO_SMALL)
                        BumpBlock();
                    else
                        StartCoroutine(BreakBlock());
                }

            }
        }

        private void BumpBlock()
        {
            animator.SetTrigger("bump");
            
        }

        private IEnumerator BreakBlock()
        {
            breakParticle.Play();
            brickSprite.SetActive(false);
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }

            //Kill Entity On Top If Any

            yield return new WaitForSeconds(breakParticle.main.startLifetime.constantMax);

            Destroy(gameObject);

        }

    }
}
