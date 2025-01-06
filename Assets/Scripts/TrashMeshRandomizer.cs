using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMeshRandomizer : MonoBehaviour
{
    private void Awake()
    {
        int childCount = this.transform.childCount;

        if (childCount <= 1)
        {
            Debug.LogWarning("Not enough children to randomize.");
            return;
        }

        int randomIndex = Random.Range(0, childCount - 1);

        for (int i = 0; i < childCount - 1; i++)
        {
            Transform child = this.transform.GetChild(i);

            if (i == randomIndex)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        Transform lastChild = this.transform.GetChild(childCount - 1);
        lastChild.gameObject.SetActive(true);
    }
}
