using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Athena.Mario.Entities;
using FrostyScripts.Misc;
using System;

namespace Athena.Mario.RenderScripts
{
[System.Serializable]
    [CreateAssetMenu(fileName ="EntityPalleteDataMap",menuName = "Mario/Palette/EntityPaletteDataMap", order = 3)]
    public class EntityPaletteDataMap : ScriptableObject
    {
        private Dictionary<EntityIdentifierEnum,EntityPaletteData> paletteDataMap;
        [SerializeField] private List<FrostyExtensions.SerializableKVPair<EntityIdentifierEnum,EntityPaletteData>> paletteDataMapList;

        public void Start(){
            //GenerateMap();
        }

        public void GenerateMap(){
            paletteDataMap=paletteDataMapList.ToDictionary(x=>x.key,x=>x.value);
        }
        public EntityPaletteData GetEntityPaletteData(EntityIdentifierEnum entityIdentifier)
        {
            return paletteDataMap[entityIdentifier];
        }
    }}