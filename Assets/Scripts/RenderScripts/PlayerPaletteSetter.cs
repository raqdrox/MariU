using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.RenderScripts
{

    public class PlayerPaletteSetter : PaletteSetter
    {

        [SerializeField]SpriteRenderer deadRenderer;
        [SerializeField]SpriteRenderer smallRenderer;
        [SerializeField]SpriteRenderer bigRenderer;
        [SerializeField]SpriteRenderer fireRenderer;

        override protected void Awake()
        {
            deadRenderer.material=Instantiate(paletteMaterial);
            smallRenderer.material=Instantiate(paletteMaterial);
            bigRenderer.material=Instantiate(paletteMaterial);
            fireRenderer.material=Instantiate(paletteMaterial);
            
            base.Awake();
            InitPalette();
        }

        override public void InitPalette(){
            SetVariant(currentActiveVariant);
            //SetPlayerEmissiveIntensity(emissiveIntensity);
        }



        
        public void SetVariant(string variantName){
            //variantName=[base,fire]

            if(variantName!="base" && variantName!="fire") return;

            SetPaletteByVariant(variantName,deadRenderer);
            SetPaletteByVariant(variantName,smallRenderer);
            SetPaletteByVariant(variantName,bigRenderer);
            SetPaletteByVariant(variantName,fireRenderer);

            
            currentActiveVariant=variantName;
        }

        private void SetPlayerEmissiveIntensity(float intensity){
            SetEmissiveIntensity(intensity,deadRenderer);
            SetEmissiveIntensity(intensity,smallRenderer);
            SetEmissiveIntensity(intensity,bigRenderer);
            SetEmissiveIntensity(intensity,fireRenderer);
        }



    }

}
