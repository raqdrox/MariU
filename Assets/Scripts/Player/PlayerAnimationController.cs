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
        [SerializeField]private String[] starEffectVariantNames;

        

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
            


            Sequence starFlash = DOTween.Sequence();
            starFlash.SetId("starFlash");
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