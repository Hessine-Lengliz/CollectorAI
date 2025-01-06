using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public GameObject cam;
    private void Update()
    {
       
            Vector3 direction = cam.transform.position - transform.position;

            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(-direction);
            }
        
    }
}
