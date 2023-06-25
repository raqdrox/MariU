using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Athena.Mario.Entities;
namespace Athena.Mario.RenderScripts
{

    public class PaletteManager : MonoBehaviour
    {
        [SerializeField]private Palette _palette;

        [SerializeField]public UnityEvent ResetPaletteEvent;
        
        [SerializeField]private EntityPaletteDataMap levelPaletteData;
        public static PaletteManager Instance { get; private set; }
        PaletteManager()
        {
            Instance = this;
        }
        private void Awake(){
            ResetPaletteEvent.AddListener(()=>Debug.Log("Palette Reset"));
        }
        public List<Color> GetPaletteColors(int id)
        {
            return _palette.GetColors(id);
        }
        
        public EntityPaletteData GetEntityPaletteDataFromLevelPaletteData(EntityIdentifierEnum entityIdentifier){
            return levelPaletteData.GetEntityPaletteData(entityIdentifier);
        }
        

    }
}
