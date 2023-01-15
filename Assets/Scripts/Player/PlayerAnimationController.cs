using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Athena.Mario.Player
{
    
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField]private PlayerController playerInstance;
        [SerializeField]private Shader[] starEffectPalletes;

        void Awake()
        {
            DOTween.Init();

    }
        public void skid(bool toggle)
        {
            playerInstance.animator.SetBool("skid", toggle);
        }

        public void deathAnimation()
        {}
        
        public void starFlash(SpriteRenderer target,float duration, TweenCallback onComplete = null)
        {
            var gradient = new Gradient();
            var colorKey = new GradientColorKey[5];
            colorKey[0].color = Color.white;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.green;
            colorKey[1].time = 0.25f;
            colorKey[2].color = Color.red;
            colorKey[2].time = 0.50f;
            colorKey[3].color = Color.black;
            colorKey[3].time = 0.75f;
            colorKey[4].color = Color.white;
            colorKey[4].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            var alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            Sequence starFlash = DOTween.Sequence();
            starFlash.SetId("starFlash");
            starFlash.Append(target.DOGradientColor(gradient, 1f));
            //starFlash.AppendInterval(interval);
            starFlash.SetLoops((int)(duration/0.2f));
            starFlash.OnComplete(onComplete);
            starFlash.Play();
            
            
        }
        
            
        
        public void cooldownFlash(SpriteRenderer target,float duration, TweenCallback onComplete = null)
        {
            Sequence coolFlash = DOTween.Sequence();
            coolFlash.SetId("starFlash");
            coolFlash.Append(target.DOFade(0.5f, 0.1f));
            coolFlash.Append(target.DOFade(1f, 0.1f));
            //coolFlash.AppendInterval(interval);
            coolFlash.SetLoops((int)(duration/0.2f));
            coolFlash.OnComplete(onComplete);
            coolFlash.Play();

        }

    }


}