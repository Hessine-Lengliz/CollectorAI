using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdHocCharacter : MonoBehaviour
{
    public AgentManager _agentManager;
    public Animator _animator;

    public Transform handTransform;
    public Transform trashBagTransform;
    public NavMeshAgent navMeshAgent;
    public Transform targetItem;
    public int currentTrashCount = 0;
    public bool currentlyOnTrashcan = false;
    public bool pickUpTrash = false;
    public int currentValue = 0;
    public bool moveOn = false;
    public bool gotUp = true;
    public float upPushForce = 1.5f;
    public int TotalTrashCollected = 0;
    void Start()
    {
        SwitchTarget();
    }

    public void SwitchTarget()
    {
        // Reset target
        targetItem = null;

        if (!currentlyOnTrashcan)
        {
            // Find the closest trash piece
            Transform trashToSelect = null;
            float closestDistance = float.MaxValue;

            foreach (Trash item in _agentManager.trashPieces)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    trashToSelect = item.transform;
                }
            }

            // Set the target item to the closest trash piece
            targetItem = trashToSelect;
        }
        else
        {
            // Find the closest trash can
            Transform closestTrashCan = null;
            float closestDistance = float.MaxValue;

            foreach (Trash item in _agentManager.trashCans)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTrashCan = item.transform;
                }
            }

            // Set the target item to the closest trash can
            targetItem = closestTrashCan;
        }

        // If we have a valid target, set the destination
        if (targetItem != null)
        {
            navMeshAgent.SetDestination(targetItem.position);
            _animator.SetTrigger("Walk");
        }
    }

    //Additions

    public void OnTrashPickedUpChangeTarget()
    {
        if (currentlyOnTrashcan)
        {
            _animator.SetTrigger("Throw");
            currentlyOnTrashcan = !currentlyOnTrashcan;
            TotalTrashCollected += currentTrashCount;
            currentTrashCount = 0;
        }
        else
        {
            _animator.SetTrigger("PickUp");
            currentTrashCount++;
        }

        if (currentTrashCount >= _agentManager.trashCapacity)
        {
            currentlyOnTrashcan = !currentlyOnTrashcan;
           // currentTrashCount = 0;
        }

        StartCoroutine(OnTrashPickedUpChangeTargetContinuation());
    }

    private IEnumerator OnTrashPickedUpChangeTargetContinuation()
    {
        yield return new WaitUntil(() => moveOn);
        moveOn = false;

        SwitchTarget();
    }

    public void GetHitByDog()
    {
        gotUp = false;
        navMeshAgent.ResetPath();
        _animator.SetTrigger("GetHit");
        currentTrashCount = 0;
        StartCoroutine(AwaitGetUp());
    }

    private IEnumerator AwaitGetUp()
    {
        List<Transform> trashChildList = new List<Transform>();
        foreach (Transform trashChild in trashBagTransform)
        {
            trashChildList.Add(trashChild);
        }

        foreach (Transform trashChild in trashChildList)
        {
            trashChild.parent = null;
            Rigidbody rb = trashChild.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            rb.isKinematic = false;

            Vector3 randomDirection = Vector3.up * upPushForce + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0, UnityEngine.Random.Range(-0.5f, 0.5f));
            rb.AddForce(randomDirection.normalized * 20f, ForceMode.Impulse);

            StartCoroutine(TrashReset(trashChild));
        }


        yield return new WaitUntil(() => gotUp);

        navMeshAgent.ResetPath();
        moveOn = true;


        // agentCharacter.StartMove(oldPath);
        SwitchTarget();
    }

    private IEnumerator TrashReset(Transform trashChild)
    {
        Trash _coin = trashChild.GetComponent<Trash>();
        yield return new WaitForSeconds(2.25f);
        Rigidbody rb = trashChild.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.useGravity = false;
        rb.isKinematic = true;

        Rigidbody newRb = trashChild.GetChild(0).GetComponent<Rigidbody>();
        newRb.isKinematic = false;
        newRb.useGravity = true;
        newRb.constraints = RigidbodyConstraints.None;

        _coin.hasBeenPickedUp = false;

        _coin.ReStart(this);

        _coin.transform.GetChild(1).gameObject.SetActive(true);
        _agentManager.trashPieces.Add(_coin);
    }
}