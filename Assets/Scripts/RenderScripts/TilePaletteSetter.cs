using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.RenderScripts
{
    public class TilePaletteSetter : PaletteSetter
    {
        [SerializeField] Renderer tileRenderer;

        
        override protected void Awake()
        {
            tileRenderer.material=Instantiate(paletteMaterial);
            base.Awake();
            InitPalette();
            
        }


        override public void InitPalette(){
            SetVariant(currentActiveVariant);
            SetTileEmissiveIntensity(emissiveIntensity);
        }

        public void SetVariant(string variantName){
            SetPaletteByVariant(variantName,tileRenderer);

        }

        private void SetTileEmissiveIntensity(float intensity){
            SetEmissiveIntensity(intensity,tileRenderer);
        }
        
    }
}
