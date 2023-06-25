using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Athena.Mario.Entities;
namespace Athena.Mario.RenderScripts
{
    [System.Serializable]
    [CreateAssetMenu(fileName ="EntityPalleteData",menuName = "Mario/Palette/EntityPaletteData", order = 3)]

    public class EntityPaletteData :ScriptableObject
    {
        [SerializeField]private List<PaletteVariant> variantData; 
        
        public PaletteVariant GetPaletteVariant(string variantName){
            var variant= variantData.Find(x => x.name==variantName);
            if(variant.paletteId==0 && variant.name=="")
                variant.name="invalid";
            return variant;
        }
    }

    
}
