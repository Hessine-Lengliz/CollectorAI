using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonteCarloAgent : MonoBehaviour
{
    public AgentManager _agentManager;
    public Animator _animator;

    public float decisionTime = 1f; // Time for MCTS to decide the best path
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
        StartCoroutine(RunMCTS());
    }

    public IEnumerator RunMCTS()
    {
        float bestScore = float.MinValue;
        Transform bestItem = null;

        // Start Monte Carlo Tree Search to find the best item
        float startTime = Time.time;
        while (Time.time - startTime < decisionTime)
        {
            if (!currentlyOnTrashcan)
            {
                foreach (Trash item in _agentManager.trashPieces)
                {
                    float score = Simulate(item.transform);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestItem = item.transform;
                    }
                }
            }
            else
            {
                foreach (Trash item in _agentManager.trashCans)
                {
                    float score = Simulate(item.transform);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestItem = item.transform;
                    }
                }
            }
            
            yield return null;
        }

        targetItem = bestItem;
        if (targetItem != null)
        {
            navMeshAgent.SetDestination(targetItem.position);
            _animator.SetTrigger("Walk");
        }
    }

    float Simulate(Transform item)
    {
        // Simple simulation: use the negative distance as the score
        float distance = Vector3.Distance(transform.position, item.position);
        return -distance; // Closer items have a higher score
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
          
            //currentTrashCount = 0;
        }

        StartCoroutine(OnTrashPickedUpChangeTargetContinuation());
    }

    private IEnumerator OnTrashPickedUpChangeTargetContinuation()
    {
        yield return new WaitUntil(() => moveOn);
        moveOn = false;

        StartCoroutine(RunMCTS());
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
        StartCoroutine(RunMCTS());
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