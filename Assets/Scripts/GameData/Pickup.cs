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
        [SerializeField]protected float _despawnTime = 30f;
        [SerializeField] protected bool neverDespawn = false;
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
            if(!neverDespawn)
                pickupKillTimer?.Tick(Time.deltaTime);
        }



        abstract protected void PickupPayload(Collider2D picker);

        virtual public void EnablePickup(bool enable)
        {
            isEnabled = enable;
            SetDespawnTimer();
        }

        private void SetDespawnTimer()
        {
            if (neverDespawn)
                return;
            if (pickupKillTimer != null)
            {
                pickupKillTimer.OnTimerEnd -= PickupExpire;
                pickupKillTimer = null;
            }
            pickupKillTimer = new Timer(_despawnTime);
            pickupKillTimer.OnTimerEnd += PickupExpire;
        }

        virtual protected void PickupExpire()
        {
            Destroy(gameObject);
        }

    }
}