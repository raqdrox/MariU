using Athena.Mario.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Athena.Mario.Misc
{
    public class MarioDebugScript : MonoBehaviour
    {
        [SerializeField] Text Statetxt;
        [SerializeField] Text Effecttxt;
        private PlayerController _currentPlayer;

        private void Start()
        {
            _currentPlayer = FindObjectOfType<PlayerController>();
            if(_currentPlayer!=null)
                StateChangeDebug(_currentPlayer.CurrentPlayerState);
        }

        public void SetTimescale(float t)
        {
            Time.timeScale = t;
        }

        public void StateChangeDebug(int stateId)
        {
            _currentPlayer?.SetPlayerState((PlayerStates)stateId);
            StateDebugUpdate();
        }
        public void StateChangeDebug(PlayerStates state)
        {
            _currentPlayer?.SetPlayerState(state);
            StateDebugUpdate();
        }

        public void StateDebugUpdate()
        {
            Statetxt.text = _currentPlayer != null?_currentPlayer.CurrentPlayerState.ToString(): Statetxt.text;
        }
        private void FixedUpdate()
        {
            StateDebugUpdate();
        }
        
        public void SetEffect(int effect)
        {
            Effecttxt.text = ((PowerEffects)effect).ToString();
            _currentPlayer.SetEffect((PowerEffects)effect,10);
        }
        
    }
}
