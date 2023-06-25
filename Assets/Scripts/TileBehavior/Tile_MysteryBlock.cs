using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Items;
using Athena.Mario.Player;
using Athena.Mario.Entities;

namespace Athena.Mario.Tiles
{
    public enum MysteryItem
    {
        MYS_COIN,
        MYS_POWERUP,
        MYS_LIFE,
        MYS_POWERDOWN,
        MYS_STAR,
    }
    public class Tile_MysteryBlock : Entity
    {
        public override EntityIdentifierEnum entityIdentifier => EntityIdentifierEnum.TILE_MYSBLOCK;

        bool isCollected = false;
        [SerializeField] float activationPower = 2f;
        [SerializeField] int MaxUses = 1;
        int Uses = 0;
        [SerializeField] Sprite UsedSprite;
        [SerializeField] Transform ItemSpawnPoint;
        [SerializeField] SpriteRenderer spriteRenderer;

        [SerializeField] MysteryItem SpawnItemType;
        [SerializeField] SpawnableData Spawnables;
        [SerializeField] Animator animator;
        [SerializeField] Animator coinAnimator;
        [SerializeField] float itemSpawnTime = 5f;
        
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
            if (collision.otherCollider.GetType() == typeof(PolygonCollider2D) && collision.collider.gameObject.GetComponent<PlayerController>()!= null)
            {

                if (collision.relativeVelocity.y > activationPower)
                    TriggerBlock(collision.collider.gameObject.GetComponent<PlayerController>());

            }
        }

        void TriggerBlock(PlayerController plr)
        {

            Uses += 1;
            if (Uses >= MaxUses)
            {
                SetBlockUsed();
            }
            animator.SetTrigger("bump");
            ItemType type=ItemType.ITEM_MUSHROOM;
            switch (SpawnItemType)
            {
                case MysteryItem.MYS_COIN:
                    //coinAnimator.transform.position = transform.position;
                    //coinAnimator.SetTrigger("coin");
                    type = ItemType.ITEM_COIN;
                    break;
                case MysteryItem.MYS_POWERUP:
                    if (plr.CurrentPlayerState == PlayerStates.MARIO_SMALL)
                        type = ItemType.ITEM_MUSHROOM;
                    else
                        type = ItemType.ITEM_FLOWER;
                    break;
                case MysteryItem.MYS_LIFE:
                    return;
                case MysteryItem.MYS_POWERDOWN:
                    return;
                case MysteryItem.MYS_STAR:
                    type = ItemType.ITEM_STAR;
                    break;
                default:
                    break;
            }
            
            var spawnedObj = Instantiate(Spawnables.GetPrefabFor(type), blockIdlePos, ItemSpawnPoint.rotation);
            var spawnedItem = spawnedObj.GetComponent<ISpawnableItem>();
            spawnedItem.OnStartSpawn();
            if (spawnedItem.NeedsSpawnCycle)
            { StartCoroutine(ItemSpawnSequence(spawnedObj)); }
        }

    

        IEnumerator ItemSpawnSequence(GameObject spawnedObj)
        {
            float currentMovementTime = 0f;
            while (Vector3.Distance(spawnedObj.transform.position, ItemSpawnPoint.position) > 0)
            {
                currentMovementTime += Time.deltaTime;
                spawnedObj.transform.position = Vector3.Lerp(blockIdlePos, ItemSpawnPoint.position, currentMovementTime / itemSpawnTime);

                //transform.position = Vector3.Lerp(blockIdlePos, ItemSpawnPoint.position, currentMovementTime / itemSpawnTime);//Funny Mistake
                yield return null;
            }
            EnableSpawnedItem(spawnedObj);
        }

        void EnableSpawnedItem(GameObject spawnedObj)
        {
            
            if (spawnedObj == null)
                return;
            spawnedObj.GetComponent<ISpawnableItem>().OnEndSpawn();
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