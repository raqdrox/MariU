using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using FrostyScripts.Misc;
using UnityEngine.Events;

namespace Athena.Mario.Items
{
    public abstract class Pickup : MonoBehaviour
    {
        [SerializeField]protected bool isEnabled = false;
        protected float DespawnTime = 30f;

        private Timer pickupKillTimer=null;
         

        virtual protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && isEnabled)
            {
                PickupPayload(other);
            }
        }

        virtual protected void FixedUpdate()
        {
            pickupKillTimer?.Tick(Time.deltaTime);
        }



        abstract protected void PickupPayload(Collider2D picker);

        virtual public void EnablePickup(bool enable)
        {
            isEnabled = enable;
            if (pickupKillTimer != null)
            { 
                pickupKillTimer.OnTimerEnd -= PickupExpire;
                pickupKillTimer = null;
            }
            pickupKillTimer = new Timer(DespawnTime);
            pickupKillTimer.OnTimerEnd += PickupExpire;
        }
        virtual protected void PickupExpire()
        {
            Destroy(gameObject);
        }

    }
}