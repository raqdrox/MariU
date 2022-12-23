using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using FrostyScripts.Misc;

namespace Athena.Mario.Items
{
    class FlowerPickup : Pickup
    {


        protected override void PickupPayload(Collider2D picker)
        {
            PlayerManager player = picker.GetComponent<PlayerManager>();
            player.PowerUp();
            OnPickupExpire();
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

