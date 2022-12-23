using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]private PlayerController playerInstance;

        public PlayerController PlayerInstance { get => playerInstance; set => playerInstance = value; }


        public void TogglePlayer(bool enable)
        {
            PlayerInstance.enableMovement = enable;
            PlayerInstance.playerCollider.enabled = enable;
            PlayerInstance.rb.simulated = enable;
            PlayerInstance.rb.velocity = Vector2.zero;
        }
        public void MovePlayerToPosition(Vector3 pos)
        {
            PlayerInstance.transform.position = pos;
        }
        private void PlayerDie()
        {
            StartCoroutine(MarioDeathSequence());
        }
        private IEnumerator MarioDeathSequence()
        {
            TogglePlayer(false);
            
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
        public void SetEffect(PowerEffects effect,float time=0f)
        {
            PlayerInstance.ActiveEffect = effect;
            switch (effect)
            {
                case PowerEffects.EFFECT_SINV:
                    StartCoroutine(SInvEffect(time));
                    break;

                case PowerEffects.EFFECT_CINV:
                    StartCoroutine(CInvEffect(time));
                    break;
                default:
                    break;
            }
        }
        public bool IsEffectActive(PowerEffects effect)
        {
            return PlayerInstance.ActiveEffect == effect;
        }
        private IEnumerator SInvEffect(float time)
        {
            PlayerInstance.IsInvincible = true;
            
            var currTime = 0f;
            var currSwitchTime = 0f;
            var switchTime = 0.1f;
            var switchMode = false;
            while (currTime < time)
            {
                var ren = PlayerInstance.GetCurrentActiveRenderer();

                if (currSwitchTime >= switchTime)
                {
                    switchMode = !switchMode;
                    currSwitchTime = 0f;
                    ren.color = switchMode? Color.red : Color.white;
                }
                currSwitchTime += Time.deltaTime;
                currTime += Time.deltaTime;
                yield return null;
            }
            PlayerInstance.ResetPlayerRenderers();
            PlayerInstance.IsInvincible = false;
            SetEffect(PowerEffects.EFFECT_NONE);
        }
        private IEnumerator CInvEffect(float time)
        {
            PlayerInstance.IsInvincible = true;
            Physics2D.IgnoreLayerCollision(10,11,true);
            var currTime = 0f;
            var currSwitchTime = 0f;
            var switchTime = 0.1f;
            var switchMode = false;
            while (currTime < time)
            {
                var renderer = PlayerInstance.GetCurrentActiveRenderer();
                if (currSwitchTime >= switchTime)
                {
                    switchMode = !switchMode;
                    var col = renderer.color;
                    col.a = switchMode? 0f:0.7f;
                    renderer.color = col;
                    
                }
                currSwitchTime += Time.deltaTime;
                currTime += Time.deltaTime;
                yield return null;
            }
            PlayerInstance.ResetPlayerRenderers();
            Physics2D.IgnoreLayerCollision(10,11,false);
            PlayerInstance.IsInvincible = false;
            SetEffect(PowerEffects.EFFECT_NONE);
        }
    
        public void BounceOff()
        {
            PlayerInstance.rb.AddForce(Vector2.up * PlayerInstance.bounceForce, ForceMode2D.Impulse);
        }
        public void BounceOff(float force)
        {
            PlayerInstance.rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        public void GetHit()
        {
            if (!PlayerInstance.IsInvincible && PlayerInstance.CurrentPlayerState != PlayerStates.MARIO_DEAD) 
                PowerDown();
        }
        public void PowerUp()
        {

            if (PlayerInstance.CurrentPlayerState == PlayerStates.MARIO_SMALL)
            {
                PlayerInstance.SetPlayerState(PlayerStates.MARIO_BIG);
            }
            else
            {
                PlayerInstance.SetPlayerState(PlayerStates.MARIO_FIRE);
            }

        }
        public void PowerDown()
        {
            if (PlayerInstance.CurrentPlayerState == PlayerStates.MARIO_SMALL)
            {
                PlayerInstance.SetPlayerState(PlayerStates.MARIO_DEAD);
            }
            else
            {
                PlayerInstance.SetPlayerState(PlayerStates.MARIO_SMALL);
                SetEffect(PowerEffects.EFFECT_CINV, PlayerInstance.invTime);
            }
        }

    }
}