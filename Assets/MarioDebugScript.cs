using Athena.Mario.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Athena.Mario.Misc
{
    public class MarioDebugScript : MonoBehaviour
    {
        public static MarioDebugScript Instance;
        [SerializeField] Text Statetxt;
        [SerializeField] Text Effecttxt;
        [SerializeField]private PlayerManager _currentPlayerManager;
void Awake()
{
    Instance = this;
}
        private void Start()
        {
            if(_currentPlayerManager!=null)
                StateChangeDebug(_currentPlayerManager.PlayerInstance.CurrentPlayerState);
        }

        public void SetTimescale(float t)
        {
            Time.timeScale = t;
        }

        public void StateChangeDebug(int stateId)
        {
            _currentPlayerManager?.PlayerInstance.SetPlayerState((PlayerStates)stateId);
            StateDebugUpdate();
        }
        public void StateChangeDebug(PlayerStates state)
        {
            _currentPlayerManager?.PlayerInstance.SetPlayerState(state);
            StateDebugUpdate();
        }

        public void StateDebugUpdate()
        {
            Statetxt.text = _currentPlayerManager != null?_currentPlayerManager.PlayerInstance.CurrentPlayerState.ToString(): Statetxt.text;
        }
        private void FixedUpdate()
        {
            StateDebugUpdate();
        }
        
        public void SetEffect(int effect)
        {
            Effecttxt.text = ((PowerEffects)effect).ToString();
            _currentPlayerManager.SetEffect((PowerEffects)effect,10);
        }

        public void BouncePlayer()
        {
            _currentPlayerManager.BounceOff();
        }
    }
}
