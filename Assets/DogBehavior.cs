using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;

public class DogBehavior : MonoBehaviour
{
    public Animator _animator;
    public Transform player;
    public NavMeshAgent agent;
    public float patrolRadius = 10f;
    public float chaseDistance = 1.5f;

    public Transform backupPointsParent;
    public List<Transform> backupPoints = new List<Transform>();

    private bool biteFinished = true;
    private bool isChasing = false;
    private bool isPatrolling = true;

    private void Start()
    {
        foreach(Transform child in backupPointsParent)
        {
            backupPoints.Add(child);
        }

        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        while (isPatrolling)
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;
            NavMeshHit hit;

            bool foundValidPosition = false;

            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
            {
                if (!NavMesh.Raycast(transform.position, hit.position, out _, NavMesh.AllAreas))
                {
                    foundValidPosition = true;
                    agent.SetDestination(hit.position);
                }
            }

            if (foundValidPosition)
            {
                _animator.SetBool("isWalking", true);
                _animator.SetBool("isIdle", false);
                _animator.SetBool("isRunning", false);

                yield return new WaitUntil(() => Vector3.Distance(transform.position, hit.position) < 1f);
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isIdle", true);
                _animator.SetBool("isRunning", false);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerMonteCarlo") || other.CompareTag("PlayerAdHoc") && !isChasing)
        {
            player = other.transform;
            isChasing = true;
            isPatrolling = false;
            StopCoroutine(Patrol());
            StartCoroutine(ChasePlayer());
        }
    }

    private IEnumerator ChasePlayer()
    {
        while (isChasing)
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) < chaseDistance && biteFinished)
            {
                if (player.parent.GetComponent<AgentManager>())
                {
                    yield return new WaitUntil(() => player.parent.GetComponent<AgentManager>()._animator.GetCurrentAnimatorStateInfo(0).IsName("WalkForward"));
                }
                else
                {
                    if (player.GetComponent<MonteCarloAgent>())
                    {
                        yield return new WaitUntil(() => player.GetComponent<MonteCarloAgent>()._animator.GetCurrentAnimatorStateInfo(0).IsName("WalkForward"));
                    }
                    else
                    {
                        yield return new WaitUntil(() => player.GetComponent<AdHocCharacter>()._animator.GetCurrentAnimatorStateInfo(0).IsName("WalkForward"));
                    }
                }
                
                yield return new WaitForSeconds(0.25f);
                biteFinished = false;
                _animator.SetTrigger("Bite");

                yield return new WaitUntil(() => biteFinished);

                StartCoroutine(GoToBackupPoint());
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator GoToBackupPoint()
    {
        Transform furthestPoint = null;
        float maxDistance = 0f;

        foreach (Transform point in backupPoints)
        {
            float distance = Vector3.Distance(transform.position, point.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestPoint = point;
            }
        }

        if (furthestPoint != null)
        {
            agent.SetDestination(furthestPoint.position);
        }

        yield return new WaitUntil(() => Vector3.Distance(transform.position, furthestPoint.position) < 1f);

        isChasing = false;
        isPatrolling = true;
        StartCoroutine(Patrol());
    }

    public void SwitchBite()
    {
        biteFinished = true;
    }

    public void HitPlayer()
    {
        player.parent.GetComponent<AgentManager>()?.GetHitByDog();
        player.GetComponent<MonteCarloAgent>()?.GetHitByDog();
        player.GetComponent<AdHocCharacter>()?.GetHitByDog();
    }
}
