using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Player;
using FrostyScripts.Misc;
using UnityEngine.Events;

namespace Athena.Mario.Items
{
    public abstract class Pickup : MonoBehaviour, ISpawnableItem
    { 
        [SerializeField]protected bool isEnabled = false;
        [SerializeField]protected float _despawnTime = 30f;
        [SerializeField]protected bool neverDespawn = false;
        private Timer pickupKillTimer=null;
        protected Rigidbody2D rb;
        private bool DoSpawnCycle=true;
        public bool NeedsSpawnCycle => DoSpawnCycle;

        protected virtual void Awake()
        {
            rb = GetComponentInChildren<Rigidbody2D>();
            EnablePickup(false);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && isEnabled)
            {
                PickupPayload(other);
            }
        }

        protected virtual void FixedUpdate()
        {
            if(!neverDespawn)
                pickupKillTimer?.Tick(Time.deltaTime);
        }



        protected abstract void PickupPayload(Collider2D picker);

        public virtual void EnablePickup(bool enable)
        {
            isEnabled = enable;
            SetDespawnTimer(enable);
        }

        private void SetDespawnTimer(bool create)
        {
            if (neverDespawn)
                return;
            if (pickupKillTimer != null)
            {
                pickupKillTimer.OnTimerEnd -= OnPickupExpire;
                pickupKillTimer = null;
            }
            if (create)
            {
                pickupKillTimer = new Timer(_despawnTime);
                pickupKillTimer.OnTimerEnd += OnPickupExpire;
            }
        }

        virtual protected void OnPickupExpire() => Destroy(gameObject);

        public void OnStartSpawn() { }


        public void OnEndSpawn() => EnablePickup(true);
    }
}