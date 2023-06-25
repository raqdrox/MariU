using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrostyScripts.Misc
{
    public class Timer
    {
        protected float RemainingSeconds;
        public event Action OnTimerEnd;

        protected float _currTime = 0;

        public Timer(float duration)
        {
            RemainingSeconds = duration;
        }

        public void Tick(float deltaTime)
        {
            if(RemainingSeconds == 0f) { return; }
            RemainingSeconds -= deltaTime;
            CheckForTimerEnd();
        }

        private void CheckForTimerEnd()
        {
            if (RemainingSeconds > 0f) { return; }

            RemainingSeconds = 0f;

            OnTimerEnd?.Invoke();
        }

        
    }
}