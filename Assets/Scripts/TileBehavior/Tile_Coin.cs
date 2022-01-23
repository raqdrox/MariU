using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.Tiles
{
    public class Tile_Coin : MonoBehaviour
    {
        [SerializeField]int points = 100;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                print("Coin Collected worth " + points + " Points");
                gameObject.SetActive(false);
            }
        }
    }
}