using UnityEngine;

namespace FrostyScripts.CameraScripts
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 targetOffset;
        private void Update()
        {
            try
            {
                var position = target.position;
                var transform1 = transform;
                transform1.position = new Vector3(position.x, position.y, transform1.position.z)+targetOffset;
            }
            catch(MissingReferenceException)
            {
                Debug.LogError("Missing Target");
            }
        }
        
    }
}
