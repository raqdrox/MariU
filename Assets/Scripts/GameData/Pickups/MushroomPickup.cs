using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using FrostyScripts.Misc;

namespace Athena.Mario.Items
{
    public class MushroomPickup : Pickup
    {
        [SerializeField] int MoveDirection = 0;
        [SerializeField] float moveSpeed = 1f;
        Rigidbody2D rb;
        [SerializeField] LayerMask DetectLayer;
        [SerializeField] private float sideOffset = 0.2f;
        [SerializeField] new float DespawnTime = 10f;


        private void Awake()
        {
            rb = GetComponentInChildren<Rigidbody2D>();
            EnablePickup(false);
            EnablePickup(true);

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

        }

        private void CheckForSideHit()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(MoveDirection, 0f), sideOffset,DetectLayer);//|| Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundMask);
            if (hit.distance < 0.7f && hit.collider)
            {
                print(hit.collider);
                MoveDirection *= -1; 
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position , transform.position + (new Vector3(MoveDirection, 0f,0f)*sideOffset));
        }

        new private void FixedUpdate()
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