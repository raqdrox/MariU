using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena
{
    public class CameraStuff 
    {
        bool InfiniteCameraCanSeePoint(Camera camera, Vector3 point)
        {
            Vector3 viewportPoint = camera.WorldToViewportPoint(point);
            return (viewportPoint.z > 0 && (new Rect(0, 0, 1, 1)).Contains(viewportPoint));
        }
    }
}
