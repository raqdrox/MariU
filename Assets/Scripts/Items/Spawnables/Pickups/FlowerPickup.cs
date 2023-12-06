using UnityEngine;
using Athena.Mario.Player;
using Athena.Mario.Entities;
using Athena.Mario.RenderScripts;
using UnityEditor.SceneManagement;
using DG.Tweening;

namespace Athena.Mario.Items
{
    class FlowerPickup : Pickup
    {
        public override EntityIdentifierEnum entityIdentifier => EntityIdentifierEnum.ITEM_FLOWER;

        [SerializeField]float animSpeed=0.2f;
        public TilePaletteSetter paletteSetter;

        override protected void Awake()
        {
            base.Awake();
            if (paletteSetter == null)
                paletteSetter=GetComponent<TilePaletteSetter>();
            SetupAnimation();
            
        }
        protected override void PickupPayload(Collider2D picker)
        {
            PlayerManager player = picker.GetComponent<PlayerManager>();
            player.PowerUp();
            OnPickupExpire();
        }

        public override void EnablePickup(bool enable)
        {
            base.EnablePickup(enable);
            Collider2D trigger = GetComponentInChildren<Collider2D>();
            trigger.enabled = enable;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            
        }

        public void SetupAnimation()
        {
            string variantName = paletteSetter.GetPaletteVariantName();

            //loop through 1 - 4 variants 
            Sequence shineSequence=DOTween.Sequence()
            .AppendCallback(()=>{
            paletteSetter.SetVariant(variantName + "1");
            })
            .AppendInterval(animSpeed)
            .AppendCallback(()=>{
            paletteSetter.SetVariant(variantName + "2");
            })
            .AppendInterval(animSpeed)
            .AppendCallback(()=>{
            paletteSetter.SetVariant(variantName + "3");
            })
            .AppendInterval(animSpeed)
            .AppendCallback(()=>{
            paletteSetter.SetVariant(variantName + "4");
            })
            .AppendInterval(animSpeed)
            .SetLoops(-1)
            .Play();



        }

    }
}

