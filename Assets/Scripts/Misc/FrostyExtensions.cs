using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrostyScripts.Misc
{
    public static class FrostyExtensions
    {
            /// <summary>
            /// Extension method to check if a layer is in a layer mask
            /// </summary>
            /// <param name="mask"></param>
            /// <param name="layer"></param>
            /// <returns></returns>
            public static bool Contains(this LayerMask mask, int layer)
            {
                return mask == (mask | (1 << layer));
            }
            
            /// <summary>
            /// Get first element of Component Type hit by RaycastHit List
            /// </summary>
            /// <param name="hits"></param>
            /// <returns></returns>
            public static Tuple<T, RaycastHit> GetFirstTypeFromRaycasts<T>(params RaycastHit[] hits)
            {
                foreach (var hit in hits)
                {
                    if (!hit.collider) continue;
                    if (hit.collider.gameObject.GetComponent<T>()!=null)
                    {
                        return new Tuple<T, RaycastHit>(hit.collider.gameObject.GetComponent<T>(), hit);
                    }
                }
                return null;
            }
            
            /// <summary>
            /// Get element List of Component Type hit by RaycastHit List
            /// </summary>
            /// <param name="hits"></param>
            /// <returns></returns>
            public static List<Tuple<T, RaycastHit>> GetTypeFromRaycasts<T>(params RaycastHit[] hits)
            {
                var res = new List<Tuple<T, RaycastHit>>();
                foreach (var hit in hits)
                {
                    if (!hit.collider) continue;
                    if (hit.collider.gameObject.GetComponent<T>()!=null)
                    {
                        res.Add( new Tuple<T, RaycastHit>(hit.collider.gameObject.GetComponent<T>(), hit));
                    }
                }
                return res;
            }
            
            
            /// <summary>
            /// Extension method to check if Camera Can See Point
            /// </summary>
            /// <param name="camera"></param>
            /// <param name="point"></param>
            /// <returns></returns>
            static bool InfiniteCameraCanSeePoint(this Camera camera, Vector3 point)
            {
                Vector3 viewportPoint = camera.WorldToViewportPoint(point);
                return (viewportPoint.z > 0 && (new Rect(0, 0, 1, 1)).Contains(viewportPoint));
            }
            
            [Serializable]
            public class SerializableKVPair<T1,T2>
            {
                public T1 key;
                public T2 value;
            }
    }
}
