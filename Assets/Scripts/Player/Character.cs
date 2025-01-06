using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Character : MonoBehaviour
{
    #region member fields
    public bool Moving = false;

    public CharacterMoveData movedata;
    public Tile characterTile;

    public bool stopMoving = false;

    public Coroutine moveCoroutine;
    [SerializeField]
    public LayerMask GroundLayerMask;
    #endregion

    private void Awake()
    {
        FindTileAtStart();
    }

    public void FindTileAtStart()
    {
        if (characterTile != null)
        {
            FinalizePosition(characterTile);
            return;
        }

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 50f, GroundLayerMask))
        {
            FinalizePosition(hit.transform.GetComponent<Tile>());
            return;
        }

        Debug.Log("Unable to find a start position");
    }

    IEnumerator MoveAlongPath(Path path)
    {
        const float MIN_DISTANCE = 0.05f;
        const float TERRAIN_PENALTY = 0.5f;

        int currentStep = 0;
        int pathLength = path.tiles.Length-1;
        Tile currentTile = path.tiles[0];
        float animationTime = 0f;

        while (currentStep <= pathLength && !stopMoving)
        {
            yield return null;

            //Move towards the next step in the path until we are closer than MIN_DIST
            Vector3 nextTilePosition = path.tiles[currentStep].transform.position;

            float movementTime = animationTime / (movedata.MoveSpeed + path.tiles[currentStep].terrainCost * TERRAIN_PENALTY);
            
            MoveAndRotate(currentTile.transform.position, nextTilePosition, movementTime);
            animationTime += Time.deltaTime;

            if (Vector3.Distance(transform.position, nextTilePosition) > MIN_DISTANCE)
                continue;

            //Min dist has been reached, look to next step in path
            currentTile = path.tiles[currentStep];
            currentStep++;
            animationTime = 0f;
        }

        stopMoving = false;

        //FinalizePosition(path.tiles[pathLength]);
    }
    
    public void StartMove(Path _path)
    {
        Moving = true;
        characterTile.Occupied = false;

        moveCoroutine = StartCoroutine(MoveAlongPath(_path));
    }

    public void FinalizePosition(Tile tile)
    {
        //transform.position = tile.transform.position;
        characterTile = tile;
        Moving = false;
        tile.Occupied = true;
        tile.occupyingCharacter = this;
    }

    void MoveAndRotate(Vector3 origin, Vector3 destination, float duration)
    {
        if (stopMoving && !Moving)
        {
            return;
        }

        transform.position = Vector3.Lerp(origin, destination, duration);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(origin.DirectionTo(destination).Flat(), Vector3.up), 0.02f * 1.946055345814035f * 2f);
    }

}