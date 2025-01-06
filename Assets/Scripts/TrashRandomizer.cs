using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashRandomizer : MonoBehaviour
{
    public Transform gridParent;
    public List<Transform> tileList = new List<Transform>();
    public GameObject trashPrefab;
    public int NumberofTrashToSpawn = 10;
    public Transform trashCanParent;

    private List<Transform> occupiedTiles = new List<Transform>();
    private void Start()
    {
        Invoke("StartWithDelay", 1f);
    }

    private void StartWithDelay()
    {
        foreach (Transform trashCan in trashCanParent)
        {
            Trash coinScript = trashCan.GetComponent<Trash>();
            if (coinScript != null && coinScript.trashTile != null)
            {
                occupiedTiles.Add(coinScript.trashTile.transform);
                coinScript.trashTile.ModifyCost();
            }
        }

        foreach (Transform grid in gridParent)
        {
            foreach (Transform tile in grid)
            {
                if (!occupiedTiles.Contains(tile))
                {
                    tileList.Add(tile);
                }
            }
        }

        SpawnCoins();
    }

    void SpawnCoins()
    {
        int coinsToSpawn = Mathf.Min(NumberofTrashToSpawn, tileList.Count);
        List<int> usedIndexes = new List<int>();

        for (int i = 0; i < coinsToSpawn; i++)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, tileList.Count);
            } while (usedIndexes.Contains(randomIndex));

            usedIndexes.Add(randomIndex);

            Transform selectedTile = tileList[randomIndex];
            Vector3 spawnPosition = selectedTile.position + Vector3.up * 0.5f;

            Instantiate(trashPrefab, spawnPosition, Quaternion.identity);
            selectedTile.GetComponent<Tile>().ModifyCostTrash();
        }


    }
}
