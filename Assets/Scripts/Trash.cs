using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public bool hasBeenPickedUp = false;
    public Tile trashTile;

    [SerializeField] LayerMask GroundLayerMask;

    public void ReStart(AgentManager _agentManager)
    {
        Debug.Log("Restarting : " + transform.GetChild(0).name);
        if (Physics.Raycast(transform.GetChild(0).position, -transform.GetChild(0).up, out RaycastHit hit, 150f, GroundLayerMask))
        {
            trashTile = hit.transform.GetComponent<Tile>();
            Debug.Log("Found a tile for : " + transform.GetChild(0).name + " its the tile : " + trashTile.name);
            transform.position = new Vector3(trashTile.transform.position.x, trashTile.transform.position.y + 0.5f, trashTile.transform.position.z);
        }
        else
        {
            Transform newPos = _agentManager.agentCharacter.characterTile.connectedTile.transform;
            transform.position = new Vector3(newPos.position.x, newPos.position.y + 0.5f, newPos.position.z);
        }
    }

    public void ReStart(MonteCarloAgent _monteCarloAgent)
    {
        Debug.Log("Restarting : " + transform.GetChild(0).name);
        if (Physics.Raycast(transform.GetChild(0).position, -transform.GetChild(0).up, out RaycastHit hit, 150f, GroundLayerMask))
        {
            trashTile = hit.transform.GetComponent<Tile>();
            Debug.Log("Found a tile for : " + transform.GetChild(0).name + " its the tile : " + trashTile.name);
            transform.position = new Vector3(trashTile.transform.position.x, trashTile.transform.position.y + 0.5f, trashTile.transform.position.z);
        }
        else
        {
            Transform newPos = _monteCarloAgent.transform;
            transform.position = new Vector3(newPos.position.x, newPos.position.y + 0.5f, newPos.position.z);
        }
    }

    public void ReStart(AdHocCharacter _adHocCharacter)
    {
        Debug.Log("Restarting : " + transform.GetChild(0).name);
        if (Physics.Raycast(transform.GetChild(0).position, -transform.GetChild(0).up, out RaycastHit hit, 150f, GroundLayerMask))
        {
            trashTile = hit.transform.GetComponent<Tile>();
            Debug.Log("Found a tile for : " + transform.GetChild(0).name + " its the tile : " + trashTile.name);
            transform.position = new Vector3(trashTile.transform.position.x, trashTile.transform.position.y + 0.5f, trashTile.transform.position.z);
        }
        else
        {
            Transform newPos = _adHocCharacter.transform;
            transform.position = new Vector3(newPos.position.x, newPos.position.y + 0.5f, newPos.position.z);
        }
    }

    public void Start()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 150f, GroundLayerMask))
        {
            trashTile = hit.transform.GetComponent<Tile>();
        }
    }

    public void OnTrashPickup(AgentManager _agentManager)
    {
        hasBeenPickedUp = true;
        transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(GetTrash(_agentManager));
    }

    public void OnTrashPickup(AdHocCharacter _adHocCharacter)
    {
        hasBeenPickedUp = true;
        transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(GetTrash(_adHocCharacter));
    }

    public void OnTrashPickup(MonteCarloAgent monteCarloAgent)
    {
        hasBeenPickedUp = true;
        transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(GetTrash(monteCarloAgent));
    }

    private IEnumerator GetTrash(AgentManager _agentManager)
    {
        _agentManager.RotateTowards(transform.GetChild(0));
        _agentManager.pickUpTrash = false;
        _agentManager.trashPieces.Remove(this);
        _agentManager.OnTrashPickedUpChangeTarget(trashTile);
        yield return new WaitUntil(() => _agentManager.pickUpTrash);

        _agentManager.pickUpTrash = false;
        Rigidbody rb = transform.GetChild(0).GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        transform.GetChild(0).transform.localRotation = Quaternion.identity;
        transform.parent = _agentManager.handTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        yield return new WaitUntil(() => _agentManager.pickUpTrash);

        _agentManager.pickUpTrash = false;
        transform.parent = _agentManager.trashBagTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private IEnumerator GetTrash(MonteCarloAgent monteCarloAgent)
    {
        monteCarloAgent.pickUpTrash = false;
        monteCarloAgent._agentManager.trashPieces.Remove(this);
        monteCarloAgent.OnTrashPickedUpChangeTarget();
        yield return new WaitUntil(() => monteCarloAgent.pickUpTrash);

        monteCarloAgent.pickUpTrash = false;
        Rigidbody rb = transform.GetChild(0).GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        transform.GetChild(0).transform.localRotation = Quaternion.identity;
        transform.parent = monteCarloAgent.handTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        yield return new WaitUntil(() => monteCarloAgent.pickUpTrash);

        monteCarloAgent.pickUpTrash = false;
        transform.parent = monteCarloAgent.trashBagTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private IEnumerator GetTrash(AdHocCharacter _adHocCharacter)
    {
        _adHocCharacter.pickUpTrash = false;
        _adHocCharacter._agentManager.trashPieces.Remove(this);
        _adHocCharacter.OnTrashPickedUpChangeTarget();
        yield return new WaitUntil(() => _adHocCharacter.pickUpTrash);

        _adHocCharacter.pickUpTrash = false;
        Rigidbody rb = transform.GetChild(0).GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        transform.GetChild(0).transform.localRotation = Quaternion.identity;
        transform.parent = _adHocCharacter.handTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        yield return new WaitUntil(() => _adHocCharacter.pickUpTrash);

        _adHocCharacter.pickUpTrash = false;
        transform.parent = _adHocCharacter.trashBagTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private IEnumerator LerpTrashToTrashCan(Transform trash, float duration)
    {
       

        Vector3 startPosition = trash.position;

        Vector3 targetPosition = transform.position;

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            trash.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        trash.position = targetPosition;

        trash.parent = transform;
    }


    private IEnumerator TrashCanCoroutine(AgentManager _agentManager)
    {
        _agentManager.RotateTowards(transform);
        _agentManager.OnTrashPickedUpChangeTarget(trashTile);
        yield return new WaitUntil(() => _agentManager.pickUpTrash);
        _agentManager.pickUpTrash = false;

        foreach (Transform child in _agentManager.trashBagTransform)
        {
            _agentManager.currentValue++;
           // _agentManager.currentTXT.text = _agentManager.currentValue.ToString();
            StartCoroutine(LerpTrashToTrashCan(child, 0.6f));
        }
        
        if(_agentManager.currentValue >= _agentManager.trashRandomizer.NumberofTrashToSpawn)
        {
            _agentManager.winImg.SetActive(true);
            yield return new WaitForSeconds(1.25f);
            Time.timeScale = 0f;
        }
    }

    private IEnumerator TrashCanCoroutine(MonteCarloAgent monteCarloAgent)
    {
        monteCarloAgent.OnTrashPickedUpChangeTarget();
        yield return new WaitUntil(() => monteCarloAgent.pickUpTrash);
        monteCarloAgent.pickUpTrash = false;

        foreach (Transform child in monteCarloAgent.trashBagTransform)
        {
            //monteCarloAgent.currentValue++;
            //monteCarloAgent.currentTXT.text = monteCarloAgent.currentValue.ToString();
            StartCoroutine(LerpTrashToTrashCan(child, 0.6f));
        }

        if (monteCarloAgent.currentValue >= monteCarloAgent._agentManager.trashRandomizer.NumberofTrashToSpawn)
        {
            //monteCarloAgent.winImg.SetActive(true);
            yield return new WaitForSeconds(1.25f);
            Time.timeScale = 0f;
        }
    }

    private IEnumerator TrashCanCoroutine(AdHocCharacter _adHocCharacter)
    {
        _adHocCharacter.OnTrashPickedUpChangeTarget();
        yield return new WaitUntil(() => _adHocCharacter.pickUpTrash);
        _adHocCharacter.pickUpTrash = false;

        foreach (Transform child in _adHocCharacter.trashBagTransform)
        {
            //monteCarloAgent.currentValue++;
            //monteCarloAgent.currentTXT.text = monteCarloAgent.currentValue.ToString();
            StartCoroutine(LerpTrashToTrashCan(child, 0.6f));
        }

        if (_adHocCharacter.currentValue >= _adHocCharacter._agentManager.trashRandomizer.NumberofTrashToSpawn)
        {
            //monteCarloAgent.winImg.SetActive(true);
            yield return new WaitForSeconds(1.25f);
            Time.timeScale = 0f;
        }
    }

    public void StartTrashcanCoroutine(AgentManager _agentManager)
    {
        StartCoroutine(TrashCanCoroutine(_agentManager));
    }

    public void StartTrashcanCoroutine(MonteCarloAgent monteCarloAgent)
    {
        StartCoroutine(TrashCanCoroutine(monteCarloAgent));
    }

    public void StartTrashcanCoroutine(AdHocCharacter _adHocCharacter)
    {
        StartCoroutine(TrashCanCoroutine(_adHocCharacter));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * 50f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasBeenPickedUp)
        {
            return;
        }

        if (other.tag == "Player")
        {
            AgentManager _agentManager = other.transform.parent.GetComponent<AgentManager>();

            if(tag == "Trash")
            {
                if (!_agentManager.currentlyOnTrashcan)
                {
                    OnTrashPickup(_agentManager);
                }
            }
            else
            {
                if (_agentManager.currentlyOnTrashcan)
                {
                    StartTrashcanCoroutine(_agentManager);
                }
            }
        }

        if (other.tag == "PlayerMonteCarlo")
        {
            MonteCarloAgent monteCarloAgent = other.GetComponent<MonteCarloAgent>();

            if (tag == "Trash")
            {
                if (!monteCarloAgent.currentlyOnTrashcan)
                {
                    OnTrashPickup(monteCarloAgent);
                }
            }
            else
            {
                if (monteCarloAgent.currentlyOnTrashcan)
                {
                    StartTrashcanCoroutine(monteCarloAgent);
                }
            }
        }

        if (other.tag == "PlayerAdHoc")
        {
            AdHocCharacter _adHocCharacter = other.GetComponent<AdHocCharacter>();

            if (tag == "Trash")
            {
                if (!_adHocCharacter.currentlyOnTrashcan)
                {
                    OnTrashPickup(_adHocCharacter);
                }
            }
            else
            {
                if (_adHocCharacter.currentlyOnTrashcan)
                {
                    StartTrashcanCoroutine(_adHocCharacter);
                }
            }
        }
    }
}