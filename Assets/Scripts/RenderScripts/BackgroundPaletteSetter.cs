using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Athena.Mario.GameManagers.Stage;

namespace Athena.Mario.RenderScripts
{
    public class BackgroundSetter : PaletteSetter
    {
        private StageStateManager stageStateManager;

        [SerializeField] private Camera mainCamera;


        override protected void Awake()
        {
            base.Awake();
            stageStateManager = FindObjectOfType<StageStateManager>();
            mainCamera = Camera.main;
        }

        protected void Start()
        {
            currentActiveVariant=stageStateManager?.StageType.ToString();
            InitPalette();
        }

        override public void InitPalette()
        {
            SetVariant(currentActiveVariant);
        }

        public void SetVariant(string variantName)
        {
            var paletteColors = GetPaletteColorsFromVariant(currentActiveVariant);
            if (paletteColors == null) return;
            mainCamera.backgroundColor = paletteColors[0];
        }
        public string GetPaletteVariantName(){
            return currentActiveVariant;
        }
        
    }
}

