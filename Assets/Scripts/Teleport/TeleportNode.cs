using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.Teleport
{
    public abstract class TeleportNode : MonoBehaviour
    {
        [SerializeField]private bool isEnterable = true;

        public virtual bool IsEnterable
        {
            get => isEnterable;
            set => isEnterable = value;
        }
        public abstract bool EnterNode(GameObject entity);
        public abstract bool ExitNode(GameObject entity);

        
    }
}
