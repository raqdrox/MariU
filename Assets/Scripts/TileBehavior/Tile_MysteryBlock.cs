using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Items;
using Athena.Mario.Player;
using Athena.Mario.Entities;
using DG.Tweening;

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
        [SerializeField] float itemSpawnTime = 5f;

        [SerializeField] Vector3 spawnStartPosOffset = new Vector3(0, 0.5f, 0);
        
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
            // animator.SetTrigger("bump");

            //TODO: Add Bump Animation
            Bump();


            ItemType type=ItemType.ITEM_MUSHROOM_GOOD;
            switch (SpawnItemType)
            {
                case MysteryItem.MYS_COIN:
                    //coinAnimator.transform.position = transform.position;
                    //coinAnimator.SetTrigger("coin");
                    type = ItemType.ITEM_COIN;
                    break;
                case MysteryItem.MYS_POWERUP:
                    if (plr.CurrentPlayerState == PlayerStates.MARIO_SMALL)
                        type = ItemType.ITEM_MUSHROOM_GOOD;
                    else
                        type = ItemType.ITEM_FLOWER;
                    break;
                case MysteryItem.MYS_LIFE:
                    type = ItemType.ITEM_MUSHROOM_LIFE;
                    break;
                case MysteryItem.MYS_POWERDOWN:
                    type = ItemType.ITEM_MUSHROOM_BAD;
                    break;
                case MysteryItem.MYS_STAR:
                    type = ItemType.ITEM_STAR;
                    break;
                default:
                    break;
            }
            
            var spawnedObj = Instantiate(Spawnables.GetPrefabFor(type), blockIdlePos + spawnStartPosOffset, ItemSpawnPoint.rotation);
            var spawnedItem = spawnedObj.GetComponent<ISpawnableItem>();
            spawnedItem.OnStartSpawn();
            if (spawnedItem.NeedsSpawnCycle)
            { ItemSpawnSequence(spawnedObj); }
        }

        void Bump()
        {
            //DOtween Bump block like in Mario
            spriteRenderer.transform.DOBlendableMoveBy(new Vector3(0, 0.5f, 0), 0.2f).SetEase(Ease.OutQuad).OnComplete(() => spriteRenderer.transform.DOBlendableMoveBy(new Vector3(0, -0.5f, 0), 0.2f).SetEase(Ease.InQuad));
            
        }

        void ItemSpawnSequence(GameObject spawnedObj)
        {
            // move the item up to the spawn point
            spawnedObj.transform.DOMove(ItemSpawnPoint.position, itemSpawnTime).SetEase(Ease.OutQuad).OnComplete(() => EnableSpawnedItem(spawnedObj));
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
            //TODO: Add Used Sprite
            //animator.SetBool("isUsed", true);
            ResetBlockPos();
            spriteRenderer.sprite = UsedSprite;
        }

        void ResetBlockPos() => transform.position = blockIdlePos;
    }
}