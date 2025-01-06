using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public RotateScript[] rotateScripts;
   public AgentManager _agentManager;

    public AdHocCharacter _adHocCharacter;
    public MonteCarloAgent _monteCarloAgent;
    public TextMeshProUGUI _agentManagerText, _adHocCharacterText, _monteCarloAgentText;
    public GameObject[] Cams;
    int index = 0;
    public TextMeshProUGUI _agentManagerCurrentTrashCountText, _adHocCharacterCurrentTrashCountText, _monteCarloAgentCurrentTrashCountText;
    public TextMeshProUGUI _agentManagerCapText, _adHocCharacterCapText, _monteCarloAgentCapText;
    // Update is called once per frame
    void Update()
    {
        _agentManagerText.text = _agentManager.TotalTrashCollected.ToString();
        _adHocCharacterText.text = _adHocCharacter.TotalTrashCollected.ToString();
        _monteCarloAgentText.text = _monteCarloAgent.TotalTrashCollected.ToString();



        _agentManagerCurrentTrashCountText.text = _agentManager.currentTrashCount.ToString();
        _adHocCharacterCurrentTrashCountText.text = _adHocCharacter.currentTrashCount.ToString();
        _monteCarloAgentCurrentTrashCountText.text = _monteCarloAgent.currentTrashCount.ToString();

        if (Input.GetKey(KeyCode.Mouse0)) {
            changeCam();
        }

    }

    private void changeCam()
    {
        Cams[index].gameObject.SetActive(false);
        index++;
        if (index == Cams.Length)
        {
            index = 0;

        }
        Cams[index].gameObject.SetActive(true);
        foreach (var rotateScript in rotateScripts) {
            rotateScript.cam = Cams[index].transform.GetChild(0).gameObject;
        }

    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        _agentManagerCapText.text = _agentManager.trashRandomizer.NumberofTrashToSpawn.ToString();
        _adHocCharacterCapText.text = _agentManager.trashRandomizer.NumberofTrashToSpawn.ToString();
        _monteCarloAgentCapText.text = _agentManager.trashRandomizer.NumberofTrashToSpawn.ToString();
    }
    
}
