using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Items;
namespace Athena.Mario.Tiles
{
    public enum ItemType
    {
        ITEM_COIN,
        ITEM_POWERUP,
        ITEM_POWERDOWN,
        ITEM_STAR
    }
    public class Tile_MysteryBlock : MonoBehaviour
    {
        bool isCollected = false;
        [SerializeField] float activationPower = 2f;
        [SerializeField] int MaxUses = 1;
        int Uses = 0;
        [SerializeField] Sprite UsedSprite;
        [SerializeField] Transform ItemSpawnPoint;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] SpriteRenderer SpawnedObjRenderer;

        [SerializeField] ItemType SpawnItemType;
        [SerializeField] int points = 100;
        [SerializeField] Animator animator;
        [SerializeField] Animator coinAnimator;
        [SerializeField] GameObject pfItem;

        GameObject SpawnedItem = null;
        Vector3 blockIdlePos;

        private void Awake()
        {
            blockIdlePos = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ResetBlockPos();
            if (isCollected)
                return;
            if (collision.otherCollider.GetType() == typeof(PolygonCollider2D))
            {
                
                if (collision.relativeVelocity.y > activationPower)
                    TriggerBlock();
                
            }
        }

        void TriggerBlock()
        {
            print(Uses >= MaxUses);
            
            Uses += 1;
            if (Uses >= MaxUses)
            { 
                SetBlockUsed();
            }
            
            if (SpawnItemType==ItemType.ITEM_COIN)
            {
                animator.SetTrigger("spawn_coin");
                coinAnimator.transform.position = transform.position;
                coinAnimator.SetTrigger("coin");
                print("Coin Collected worth " + points + " Points");
            }
            else
            {
                SpawnedItem =  Instantiate(pfItem, ItemSpawnPoint.position, ItemSpawnPoint.rotation);
                SpawnedItem.SetActive(false);
                SpawnedObjRenderer.sprite = SpawnedItem.GetComponent<IItem>().ItemSprite;
                animator.SetTrigger("spawn_item");
                Invoke("EnableSpawnedItem", 1);
            }
            
        }

        void EnableSpawnedItem()
        {
            ResetBlockPos();
            if (SpawnedItem == null)
                return;
            SpawnedItem.SetActive(true);
            SpawnedItem.GetComponent<IItem>().OnSpawn();
        }

        void SetBlockUsed()
        {

            isCollected = true;
            animator.SetBool("isUsed", true);
            ResetBlockPos();
            spriteRenderer.sprite = UsedSprite;
        }

        void ResetBlockPos() => transform.position = blockIdlePos;
    }
}