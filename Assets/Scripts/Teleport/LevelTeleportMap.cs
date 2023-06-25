using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.Teleport
{
    [Serializable]
    public class TeleportLink
    {
        public TeleportNode enter;
        public TeleportNode exit;
    }
    [CreateAssetMenu(menuName = "Mario/LevelTeleportMap")]
    public class LevelTeleportMap : ScriptableObject
    {
        //public List<KeyValuePair<TeleportNode, TeleportNode>> teleportMap;
        public List<TeleportLink> teleportMap;
    }
}
