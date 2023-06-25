using System.Collections;
using System.Collections.Generic;
using Athena.Mario.Entities;
using UnityEngine;

namespace Athena.Mario.Tiles
{
    public class Tile_Coin : Entity
    {
        [SerializeField]int points = 100;

        public override EntityIdentifierEnum entityIdentifier => EntityIdentifierEnum.TILE_MYSBLOCK;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                print("Coin Collected worth " + points + " Points");
                Destroy(gameObject);
            }
        }
    }
}