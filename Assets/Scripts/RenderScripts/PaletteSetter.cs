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
// [ExecuteInEditMode]
    public abstract class PaletteSetter : MonoBehaviour
    {
        protected PaletteManager _paletteManager;
        [SerializeField]protected float emissiveIntensity=1.0f;
        [SerializeField]protected EntityPaletteData paletteData; 

        [SerializeField]protected string currentActiveVariant;

        [SerializeField]protected Material paletteMaterial;

        virtual protected void Awake()
        {
            _paletteManager = PaletteManager.Instance;
            
        }

        abstract public void InitPalette();
            

        virtual public void SetPaletteByVariant(string variantName,Renderer renderer){
            if(renderer==null) return;
            var variant= paletteData.GetPaletteVariant(variantName);
            if(variant.name=="invalid") return;
            currentActiveVariant=variantName;
            SetPaletteById(variant.paletteId,renderer);
        }
        virtual public void SetPaletteById(int id,Renderer renderer){
            var paletteColors=_paletteManager?.GetPaletteColors(id);
            SetPaletteColors(paletteColors,renderer);
        }
        
        virtual public void SetPaletteColors(List<Color> paletteColors,Renderer renderer){
            renderer.material.SetColor("_Color1", paletteColors[0]);
            renderer.material.SetColor("_Color2", paletteColors[1]);
            renderer.material.SetColor("_Color3", paletteColors[2]);
            renderer.material.SetColor("_Color4", paletteColors[3]);
            renderer.material.SetColor("_Color5", paletteColors[4]);
        }



        virtual public void SetEmissiveIntensity(float intensity,Renderer renderer){
            if(renderer==null) return;
            for(int i=1;i<=5;i++){
                renderer.material.SetVector("_EmissionColor", Color.white * intensity);
            }

        }

        public List<Color> GetPaletteColorsFromVariant(string variantName){
            var variant= paletteData.GetPaletteVariant(variantName);
            if(variant.name=="invalid") return null;
            var paletteColors=_paletteManager?.GetPaletteColors(variant.paletteId);
            return paletteColors;
        }
  }
}
