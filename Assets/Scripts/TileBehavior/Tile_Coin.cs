using System;
using System.Collections;
using System.Collections.Generic;
using Athena.Mario.Entities;
using Athena.Mario.RenderScripts;
using UnityEngine;
using DG.Tweening;


namespace Athena.Mario.Tiles
{
    public class Tile_Coin : Entity
    {
        [SerializeField]int points = 100;

        [SerializeField] List<String> coinVariants = new();
        
        [SerializeField] float coinSpriteStateTime = 0.1f;

        [SerializeField] TilePaletteSetter tilePaletteSetter;

        public override EntityIdentifierEnum entityIdentifier => EntityIdentifierEnum.TILE_MYSBLOCK;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                print("Coin Collected worth " + points + " Points");
                Destroy(gameObject);
            }
        }

        private void Awake()
        {
            if (tilePaletteSetter == null)
            tilePaletteSetter = GetComponent<TilePaletteSetter>();

            tilePaletteSetter.SetVariant(coinVariants[0]);
            

        }

        private void Start()
        {
            //use dotween to animate the coin. change the sprite every coinSpriteStateTime seconds using the list of coin variants
            //use the tilePaletteSetter to change the sprite

            Sequence shineSequence=DOTween.Sequence().AppendInterval(coinSpriteStateTime)
            .AppendCallback(()=>tilePaletteSetter.SetVariant(coinVariants[0]))
            .AppendInterval(coinSpriteStateTime)
            .AppendCallback(()=>tilePaletteSetter.SetVariant(coinVariants[1]))
            .AppendInterval(coinSpriteStateTime)
            .AppendCallback(()=>tilePaletteSetter.SetVariant(coinVariants[2]))
            .AppendInterval(coinSpriteStateTime)
            .AppendCallback(()=>tilePaletteSetter.SetVariant(coinVariants[3]))
            .AppendInterval(coinSpriteStateTime)
            .SetLoops(-1)
            .Play();

            
        }

        public void FixedUpdate()
        {

            
            
            
            


        }


    }
}