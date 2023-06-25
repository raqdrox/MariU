using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Athena.Mario.Items
{
    [CreateAssetMenu(menuName = "Mario/Items/Spawnables")]
    public class SpawnableData : ScriptableObject
    {
        [Serializable]
        public class Spawnable
        {
            public ItemType Type;
            public GameObject ItemPrefab;
        }
        [SerializeField] List<Spawnable> ItemMap;

        public GameObject GetPrefabFor(ItemType type)
        {
            return ItemMap.Find(item => item.Type == type).ItemPrefab;
        }
    }
}