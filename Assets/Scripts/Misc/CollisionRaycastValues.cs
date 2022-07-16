using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.Misc
{
    [CreateAssetMenu(menuName = "Mario/Collision/CollisionRaycastValues")]
    public class CollisionRaycastValues : ScriptableObject
    {
        [Header("HorizontalRaycasts")] 
        public float scSideCenterOffset = 0f;
        public float scSideStartOffset = 0.2f;
        public float scSideEndOffset = 0.2f;
        public float scTopOffset = 0.5f;
        public float scBottomOffset = -0.5f;

        [Header("VerticalRaycasts")] 
        public float tcTopCenterOffset = 0f;
        public float tcTopStartOffset = 0.2f;
        public float tcTopEndOffset = 0.2f;
        public float tcLeftOffset = 0.5f;
        public float tcRightOffset = -0.5f;
    }
}
