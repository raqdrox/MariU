using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.Entities;
using DG.Tweening;
using Athena.Mario.RenderScripts;

namespace Athena.Mario.Items
{
    public class SpawnedCoin : MonoBehaviour, ISpawnableItem
    {
        [SerializeField] int pointVal = 100;
        [SerializeField] Vector3 moveHeight;
        [SerializeField] Vector3 destroyHeight;
        [SerializeField] float moveTime = 10f;
        
        [SerializeField] List<string> coinPaletteVariants = new();
        [SerializeField] List<Sprite> coinSpriteVariants = new();

        [SerializeField] TilePaletteSetter tilePaletteSetter;
        [SerializeField] float coinSpriteStateTime = 0.3f;

        [SerializeField] SpriteRenderer spriteRenderer;


        //EventStuff

        private bool DoSpawnCycle = false;
        public bool NeedsSpawnCycle => DoSpawnCycle;

        public void OnEndSpawn() { }

        public void OnStartSpawn()
        {            

            spriteRenderer = GetComponent<SpriteRenderer>();
            tilePaletteSetter = GetComponent<TilePaletteSetter>();
            tilePaletteSetter.SetVariant(coinPaletteVariants[0]);
            spriteRenderer.sprite = coinSpriteVariants[2];


            Sequence shineSequence=DOTween.Sequence()
            .AppendInterval(coinSpriteStateTime)
            .AppendCallback(()=>{
            tilePaletteSetter.SetVariant(coinPaletteVariants[0]);
            spriteRenderer.sprite = coinSpriteVariants[0];
            })
            .AppendInterval(coinSpriteStateTime)
            .AppendCallback(()=>{
            tilePaletteSetter.SetVariant(coinPaletteVariants[1]);
            spriteRenderer.sprite = coinSpriteVariants[1];
            })
            .AppendInterval(coinSpriteStateTime)
            .AppendCallback(()=>{
            tilePaletteSetter.SetVariant(coinPaletteVariants[2]);
            spriteRenderer.sprite = coinSpriteVariants[2];
            })
            .AppendInterval(coinSpriteStateTime)
            .AppendCallback(()=>{
            tilePaletteSetter.SetVariant(coinPaletteVariants[3]);
            spriteRenderer.sprite = coinSpriteVariants[3];
            
            })
            .AppendInterval(coinSpriteStateTime)
            .SetLoops(1)
            .Play();

            Vector3 startPos = transform.position;
            transform.DOMove(transform.position + moveHeight, moveTime)
            .SetEase(Ease.OutQuad)
            .OnComplete(()=>transform.DOMove(startPos+ destroyHeight, moveTime)
            .SetEase(Ease.InQuad).OnComplete(()=>{
                shineSequence.Kill();
                Destroy(gameObject);
                }));

            
            
        }



        
    }
}