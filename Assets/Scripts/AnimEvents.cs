using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public MonteCarloAgent _monteCarloAgent;
    public AgentManager _agentManager;
    public AdHocCharacter _adHocCharacter;

    public Animator _animator;

    public MonteCarloAgent new_monteCarloAgent;
    public AgentManager new_agentManager;
    public AdHocCharacter new_adHocCharacter;

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("WalkForward"))
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        }
    }

    public void MoveOn()
    {
        if (_agentManager)
        {
            _agentManager.moveOn = true;
        }
        else
        {
            if (_monteCarloAgent)
            {
                _monteCarloAgent.moveOn = true;
            }
            else
            {
                _adHocCharacter.moveOn = true;
            }
            
        }
    }

    public void CollectTrash()
    {
        if (_agentManager)
        {
            _agentManager.pickUpTrash = true;
        }
        else
        {
            if (_monteCarloAgent)
            {
                _monteCarloAgent.pickUpTrash = true;
            }
            else
            {
                _adHocCharacter.pickUpTrash = true;
            }
        }
    }

    public void GetUp()
    {
        if (_adHocCharacter)
        {
            _adHocCharacter.gotUp = true;
        }
        else
        {
            if (_agentManager)
            {
                _agentManager.gotUp = true;
            }
            else
            {
                _monteCarloAgent.gotUp = true;
            }
        }
    }

    public void RefreshTargets()
    {
        if (_agentManager)
        {
            if (_agentManager.currentTarget.transform == new_monteCarloAgent.targetItem.transform)
            {
                new_monteCarloAgent._animator.SetTrigger("Walk");
                StartCoroutine(new_monteCarloAgent.RunMCTS());
            }
            if(_agentManager.currentTarget.transform == new_adHocCharacter.targetItem.transform)
            {
                new_adHocCharacter._animator.SetTrigger("Walk");
                new_adHocCharacter.SwitchTarget();
            }
        }
        else
        {
            if (_monteCarloAgent)
            {
                if (_monteCarloAgent.targetItem.transform == new_agentManager.currentTarget.transform)
                {
                    new_agentManager._animator.SetTrigger("Walk");
                    new_agentManager.GetClosestTrash();
                    new_agentManager.agentCharacter.Moving = false;
                    new_agentManager.NavigateToTile();
                }
                if (_monteCarloAgent.targetItem.transform == new_adHocCharacter.targetItem.transform)
                {
                    new_adHocCharacter._animator.SetTrigger("Walk");
                    new_adHocCharacter.SwitchTarget();
                }
            }
            else
            {
                if (_adHocCharacter)
                {
                    if (_adHocCharacter.targetItem.transform == new_agentManager.currentTarget.transform)
                    {
                        new_agentManager._animator.SetTrigger("Walk");
                        new_agentManager.GetClosestTrash();
                        new_agentManager.agentCharacter.Moving = false;
                        new_agentManager.NavigateToTile();
                    }
                    if (_adHocCharacter.targetItem.transform == new_monteCarloAgent.targetItem.transform)
                    {
                        new_monteCarloAgent._animator.SetTrigger("Walk");
                        StartCoroutine(new_monteCarloAgent.RunMCTS());
                    }
                }
            }
        }
    }
}
