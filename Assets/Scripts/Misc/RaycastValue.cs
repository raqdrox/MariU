using System;

namespace Athena.Mario.Misc
{
    [Serializable]
    public class RaycastValue
    {
        public Direction direction;
        public float startOffset;
        public float endOffset;
        public float centerOffset;

        //public float scSideCenterOffset = 0f;
        //public float scSideStartOffset = 0.2f;
        //public float scSideEndOffset = 0.2f;
        //public float scTopOffset = 0.5f;
        //public float scBottomOffset = -0.5f;

        //[Header("VerticalRaycasts")] 
        //public float tcTopCenterOffset = 0f;
        //public float tcTopStartOffset = 0.2f;
        //public float tcTopEndOffset = 0.2f;
        //public float tcLeftOffset = 0.5f;
        //public float tcRightOffset = -0.5f;
    }
}