using System;
using System.Collections.Generic;
using UnityEngine;

namespace Athena.Mario.Misc
{
    [Serializable]
    public class HitData
    {
        public GameObject hitObject;
        public Direction hitSide;

        public HitData(GameObject hitObject, Direction hitSide)
        {
            this.hitObject = hitObject;
            this.hitSide = hitSide;
        }
    }
    public class HitHandler : MonoBehaviour
    {
        [SerializeField] public RaycastValuesList raycastValuesList;
        [SerializeField] public LayerMask detectLayer;
        [SerializeField] public LayerMask terrainLayer;
        [SerializeField] public bool showDebug;
        public Action<List<HitData>> onHit;
        public Vector3 position;

        public static readonly Dictionary<Direction, Vector3> DirectionMap=new Dictionary<Direction, Vector3>()
        {
            {Direction.TOP,Vector3.up},
            {Direction.DOWN,Vector3.down},
            {Direction.LEFT,Vector3.left},
            {Direction.RIGHT,Vector3.right}
        };


        public void OnDrawGizmos()
        {
            if (!showDebug) return;
            Gizmos.color = Color.blue;
            var pos = transform.position;
            
            foreach (var raycastValue in raycastValuesList.raycastValues)
            {
                Vector3 startPos, endPos;
                
                if(raycastValue.direction==Direction.TOP|| raycastValue.direction==Direction.DOWN){
                    startPos = pos + new Vector3(raycastValue.centerOffset,raycastValue.startOffset,0f);
                    endPos = pos + new Vector3(raycastValue.centerOffset,Mathf.Sign(raycastValue.startOffset)*(raycastValue.endOffset+Mathf.Abs(raycastValue.startOffset)),0f);
                }
                else
                {
                    startPos = pos + new Vector3(raycastValue.startOffset,raycastValue.centerOffset,0f);
                    endPos = pos + new Vector3(Mathf.Sign(raycastValue.startOffset)*(raycastValue.endOffset+Mathf.Abs(raycastValue.startOffset)),raycastValue.centerOffset,0f);
                }

                Gizmos.DrawLine(startPos, endPos);
            }
        }

        

        /*private void FixedUpdate()
        {
            
            
            var hitList = GetHits();
        
            if (hitList.Count > 0)
            {
                onHit.Invoke(hitList);
            }
        }*/
        
        public List<HitData> GetHits()
        {
            position = transform.position;
            var hitList = new List<HitData>();
            foreach (var raycastValue in raycastValuesList.raycastValues)
            {
                var hitObj = CheckRayForHit(raycastValue);
                if (hitObj != null)
                {
                    hitList.Add(new HitData(hitObj, raycastValue.direction));
                }
            }
            
            return hitList;
        }

        private GameObject CheckRayForHit(RaycastValue raycastValue)
        {
            Vector3 startPos = raycastValue.direction switch
            {
                Direction.TOP => position + new Vector3(raycastValue.centerOffset, raycastValue.startOffset, 0f),
                Direction.DOWN => position + new Vector3(raycastValue.centerOffset, raycastValue.startOffset, 0f),
                Direction.LEFT => position + new Vector3(raycastValue.startOffset, raycastValue.centerOffset, 0f),
                Direction.RIGHT => position + new Vector3(raycastValue.startOffset, raycastValue.centerOffset, 0f),
                _ => position
            };
            RaycastHit2D hit = Physics2D.Raycast(startPos, DirectionMap[raycastValue.direction], raycastValue.endOffset, detectLayer);
            if(hit)
                return hit.collider.gameObject;
            return null;
        }
    }
}
