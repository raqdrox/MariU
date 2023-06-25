using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using FrostyScripts.Misc;
using Athena.Mario.Entities;

namespace Athena.Mario.Items
{
    public class MushroomPickup : Pickup
    {
        [SerializeField] int MoveDirection = 0;
        [SerializeField] float moveSpeed = 1f;
        [SerializeField] LayerMask DetectLayer;
        [SerializeField] private float sideOffset = 0.2f;
        [SerializeField] private float topOffset = 0.5f;
        [SerializeField] private float bottomOffset = -0.5f;

        public override EntityIdentifierEnum entityIdentifier => EntityIdentifierEnum.ITEM_MUSHROOM;

        protected override void PickupPayload(Collider2D picker)
        {
            PlayerManager player = picker.GetComponent<PlayerManager>();
            player.PowerUp();
            OnPickupExpire();
        }

        public override void EnablePickup(bool enable)
        {
            base.EnablePickup(enable);
            MoveDirection = Random.Range(-1f, 1f) > 0f ? 1 : -1;

            Collider2D collider = GetComponent<Collider2D>();
            Collider2D trigger = GetComponentInChildren<Collider2D>();
            collider.enabled = enable;
            trigger.enabled = enable;
            rb.constraints = enable? RigidbodyConstraints2D.FreezeRotation:RigidbodyConstraints2D.FreezeAll;

        }

        private void CheckForSideHit()
        {
            bool hit = Physics2D.Raycast(transform.position + new Vector3(0f, topOffset, 0f), new Vector2(MoveDirection, 0f), sideOffset,DetectLayer) || Physics2D.Raycast(transform.position + new Vector3(0f, bottomOffset, 0f), new Vector2(MoveDirection, 0f), sideOffset, DetectLayer);
            if (hit)
                MoveDirection *= -1; 
        }

        private void OnDrawGizmos()
        {
            Vector3 topPos = transform.position + new Vector3(0f, topOffset, 0f);
            Vector3 botPos = transform.position + new Vector3(0f, bottomOffset, 0f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(topPos, topPos + (new Vector3(MoveDirection, 0f,0f)*sideOffset));
            Gizmos.DrawLine(botPos, botPos + (new Vector3(MoveDirection, 0f,0f)*sideOffset));
        }

        override protected void FixedUpdate()
        {
            if (isEnabled)
            {
                base.FixedUpdate();
                CheckForSideHit();
                rb.velocity = Vector2.right * MoveDirection * moveSpeed;
            }
        }

    }
}