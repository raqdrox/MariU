using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Athena
{
    public class MarioDebugScript : MonoBehaviour
    {
        
        public void SetTimescale(float t)
        {
            Time.timeScale = t;
        }
    }
}
