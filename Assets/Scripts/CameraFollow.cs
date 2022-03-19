using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    private void Update()
    {
        try
        {
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
        catch(MissingReferenceException)
        {

        }
    }
}
