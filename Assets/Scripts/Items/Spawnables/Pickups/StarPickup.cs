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

        [SerializeField] private float gcSideOffset = 0.2f;
        [SerializeField] private float gcTopOffset = 0.5f;
        [SerializeField] private float gcBottomOffset = -0.5f;
        Rigidbody2D rb;

        override protected void Awake()
        {
            base.Awake();
            rb = GetComponentInChildren<Rigidbody2D>();
        }
        protected override void PickupPayload(Collider2D picker)
        {
            PlayerController player = picker.GetComponent<PlayerController>();
            player.PowerUp();
            PickupExpire();
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

        }

        override protected void FixedUpdate()
        {
            if (isEnabled)
            {
                base.FixedUpdate();
                CheckForSideHit();
                HandleGroundHit();
            }
        }

        private void CheckForSideHit()
        {
            bool hit = Physics2D.Raycast(transform.position + new Vector3(0f, scTopOffset, 0f), new Vector2(MoveDirection, 0f), scSideOffset, DetectLayer) || Physics2D.Raycast(transform.position + new Vector3(0f, scBottomOffset, 0f), new Vector2(MoveDirection, 0f), scSideOffset, DetectLayer);
            if (hit)
                MoveDirection *= -1;
        }

        private void HandleGroundHit()
        {
            bool hit = Physics2D.Raycast(transform.position + new Vector3(0f, gcTopOffset, 0f), new Vector2(MoveDirection, 0f), gcSideOffset, DetectLayer) || Physics2D.Raycast(transform.position + new Vector3(0f, gcBottomOffset, 0f), new Vector2(MoveDirection, 0f), gcSideOffset, DetectLayer);
            if (hit)
            {
                rb.velocity = (Vector2.up*verticalSpeed)+( Vector2.right* MoveDirection * horizontalSpeed);
            }
        }
    }
}
