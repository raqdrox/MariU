using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Athena.Mario.Teleport
{
    public class TeleportHandler : MonoBehaviour
    {
        private void Awake()
        {
            Instance = this;
        }
        public static TeleportHandler Instance { get; private set; } 
        [SerializeField] private LevelTeleportMap levelTeleportMap;
        public List<TeleportLink> teleportMap;
        TeleportNode GetExitNode(TeleportNode node)
        {
           return teleportMap.FirstOrDefault(x => x.enter == node).exit;
           
        }

        public bool TeleportPlayerFromNode(TeleportNode node,GameObject entity)
        {
            //Change To Coroutine
            TeleportNode exitNode = GetExitNode(node);
            if (exitNode == null)
            {
                Debug.LogError("No Exit Node Found");
                return false;
            }
            else
            {
                exitNode.ExitNode(entity);
                return true;
            }
        }
    }
}
