using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Athena.Mario.Entities;
namespace Athena.Mario.RenderScripts
{
    [Serializable]
    public struct PaletteVariant
    {
        public string name;
        public int paletteId;
    }

    public class PaletteSetter : MonoBehaviour
    {
        //TODO: Allow Any Entity Script to change palette by using SetPalette(string variantName)
        [SerializeField]private EntityIdentifierEnum entityIdentifier;
        private Renderer _renderer;
        private PaletteManager _paletteManager;
        [SerializeField]private float emissiveIntensity=1.0f;
        [SerializeField]private EntityPaletteData paletteData; 

        [SerializeField]private string currentActiveVariant;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _paletteManager = PaletteManager.Instance;
        }


        public void SetPalette(string variantName){
            var variant= paletteData.GetPaletteVariant(variantName);
            if(variant.name=="invalid") return;
            currentActiveVariant=variantName;
            SetPalette(variant.paletteId);
        }
        public void SetPalette(int id){
            var paletteColors=_paletteManager?.GetPaletteColors(id);
            SetPalette(paletteColors);
        }
        
        public void SetPalette(List<Color> paletteColors){
            _renderer.material.SetColor("_Color1", paletteColors[0]);
            _renderer.material.SetColor("_Color2", paletteColors[1]);
            _renderer.material.SetColor("_Color3", paletteColors[2]);
            _renderer.material.SetColor("_Color4", paletteColors[3]);
            
        }
  }
}
