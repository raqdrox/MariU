using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using FrostyScripts.Misc;

namespace Athena.Mario.Items
{
    class FlowerPickup : Pickup
    {
        Rigidbody2D rb;

        private void Awake()
        {
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
            Collider2D trigger = GetComponentInChildren<Collider2D>();
            trigger.enabled = enable;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        }

    }
}
