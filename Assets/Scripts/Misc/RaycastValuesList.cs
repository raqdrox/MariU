using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.Misc
{
    [CreateAssetMenu(menuName = "Mario/Collision/RaycastValuesList")]
    public class RaycastValuesList : ScriptableObject
    {
        public List<RaycastValue> raycastValues;
    }
}
