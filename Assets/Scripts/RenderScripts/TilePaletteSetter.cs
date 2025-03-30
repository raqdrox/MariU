using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.GameManagers.Stage;

namespace Athena.Mario.RenderScripts
{
    public class TilePaletteSetter : PaletteSetter
    {
        [SerializeField] Renderer tileRenderer;

        private StageStateManager stageStateManager;

        [SerializeField] private bool overrideStagePalette=false;

        
        override protected void Awake()
        {
            tileRenderer.material=Instantiate(paletteMaterial);
            base.Awake();
            stageStateManager = FindObjectOfType<StageStateManager>();
            
        }

        protected void Start(){

            currentActiveVariant=overrideStagePalette?currentActiveVariant:stageStateManager?.StageType.ToString();
            InitPalette();
        }

        override public void InitPalette(){
            SetVariant(currentActiveVariant);
            SetTileEmissiveIntensity(emissiveIntensity);
        }

        public void SetVariant(string variantName){
            SetPaletteByVariant(variantName,tileRenderer);
        }

        public string GetPaletteVariantName(){
            return currentActiveVariant;
        }


        private void SetTileEmissiveIntensity(float intensity){
            SetEmissiveIntensity(intensity,tileRenderer);
        }
        
    }
}
