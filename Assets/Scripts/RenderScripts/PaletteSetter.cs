using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
namespace Athena.Mario.RenderScripts
{


    [RequireComponent(typeof(Renderer))]
    [ExecuteInEditMode]
    public class PaletteSetter : MonoBehaviour
    {
        private Renderer _renderer;
        private PaletteManager _paletteManager;
        


        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _paletteManager = PaletteManager.Instance;

        }

        public virtual void SetPalette(int id){
            var paletteColors=_paletteManager.GetPaletteColors(id);
            _renderer.sharedMaterial.SetColor("_Color1", paletteColors[0]);
            _renderer.sharedMaterial.SetColor("_Color2", paletteColors[1]);
            _renderer.sharedMaterial.SetColor("_Color3", paletteColors[2]);
            _renderer.sharedMaterial.SetColor("_Color4", paletteColors[3]);

        }
        
        
    }
}
