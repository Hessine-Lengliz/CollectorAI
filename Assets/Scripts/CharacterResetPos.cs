using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterResetPos : MonoBehaviour
{
    private void Update()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
    }
}
