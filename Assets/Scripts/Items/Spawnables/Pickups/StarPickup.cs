using Athena.Mario.Entities;
using Athena.Mario.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.Items
{

    public class StarPickup : Pickup
    {
        [SerializeField] int MoveDirection = 0;
        [SerializeField] float horizontalSpeed = 1f;
        [SerializeField] float verticalSpeed = 1f;
        
        //Physics
        [SerializeField] LayerMask DetectLayer;

        [SerializeField] private float scSideOffset = 0.2f;
        [SerializeField] private float scTopOffset = 0.5f;
        [SerializeField] private float scBottomOffset = -0.5f;

        [SerializeField] private float gcBotOffset = 0.2f;
        [SerializeField] private float gcLeftOffset = 0.5f;
        [SerializeField] private float gcRightOffset = -0.5f;

        //Debug
        Color sideGizmoColor = Color.blue;
        Color groundGizmoColor = Color.blue;

        public override EntityIdentifierEnum entityIdentifier => EntityIdentifierEnum.ITEM_STAR;

        protected override void PickupPayload(Collider2D picker)
        {
            PlayerManager player = picker.GetComponent<PlayerManager>();
            player.SetEffect(PowerEffects.EFFECT_STAR,20f);

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
            rb.constraints = enable ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
            InitialPush();
        }

        override protected void FixedUpdate()
        {
            if (isEnabled)
            {
                base.FixedUpdate();
                HandleSideHit();
                HandleGroundHit(); 
            }
        }

        private void HandleSideHit()
        {
            bool hit = Physics2D.Raycast(transform.position + new Vector3(0f, scTopOffset, 0f), new Vector2(MoveDirection, 0f), scSideOffset, DetectLayer) || Physics2D.Raycast(transform.position + new Vector3(0f, scBottomOffset, 0f), new Vector2(MoveDirection, 0f), scSideOffset, DetectLayer);
            if (hit)
            {
                MoveDirection *= -1;
                sideGizmoColor = Color.red; 
            }
            else
            {
                sideGizmoColor = Color.blue;
            }
            rb.velocity = new Vector2(0f, rb.velocity.y) + (horizontalSpeed
                * MoveDirection
                * Vector2.right);
        }

        private void HandleGroundHit()
        {
            bool hit = Physics2D.Raycast(transform.position + new Vector3(gcLeftOffset, 0f, 0f), Vector3.down, gcBotOffset, DetectLayer) || Physics2D.Raycast(transform.position + new Vector3(gcRightOffset, 0f, 0f), Vector3.down, gcBotOffset, DetectLayer);
            if (hit)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f) + (Vector2.up * verticalSpeed);
                groundGizmoColor = Color.red;
            }
            else
            {
                groundGizmoColor = Color.blue;
            }
        }

        private void InitialPush()
        {
            rb.velocity = (Vector2.up * verticalSpeed) + (horizontalSpeed * MoveDirection * Vector2.right);
        }
        private void OnDrawGizmos()
        {
            Vector3 stopPos = transform.position + new Vector3(0f, scTopOffset, 0f);
            Vector3 sbotPos = transform.position + new Vector3(0f, scBottomOffset, 0f);
            Gizmos.color = sideGizmoColor;
            Gizmos.DrawLine(stopPos, stopPos + (new Vector3(MoveDirection, 0f, 0f) * scSideOffset));
            Gizmos.DrawLine(sbotPos, sbotPos + (new Vector3(MoveDirection, 0f, 0f) * scSideOffset));

            Vector3 gleftPos = transform.position + new Vector3(gcLeftOffset, 0f, 0f);
            Vector3 grightPos = transform.position + new Vector3(gcRightOffset, 0f, 0f);
            Gizmos.color = groundGizmoColor;
            Gizmos.DrawLine(gleftPos, gleftPos + (Vector3.down * gcBotOffset));
            Gizmos.DrawLine(grightPos, grightPos + (Vector3.down * gcBotOffset));
        }
    }
}
