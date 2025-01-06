using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Player_FootstepManager : MonoBehaviour
{
    public CharacterAnimationManager characterAnimationManager;
    [Header("Footsteps")]
    public List<FootstepMaterialData> materialDataList;
    public AudioSource footstepAudioSource;
    public float stepInterval;
    public float minPitch = 0.9f;
    public float maxPitch = 1.3f;
    private float stepTimer = 0f;

    private void Update()
    {
        if (characterAnimationManager.animator.GetCurrentAnimatorStateInfo(0).IsName("WalkForward"))
        {
            UpdateFootsteps();
        }
    }

    void UpdateFootsteps()
    {

        float baseMoveSpeed = 1.75f;
        float baseStepInterval = 2.80f;

        stepInterval = baseStepInterval * (baseMoveSpeed / characterAnimationManager.moveSpeed);

        float speedPercentage = characterAnimationManager.velocityMagnitude;
        float baseStepMultiplier = 1f;

        float adjustedStepInterval = stepInterval * (baseStepMultiplier / Mathf.Clamp(speedPercentage, 0.1f, 1f));

        stepTimer += Time.fixedDeltaTime;

        if (stepTimer >= adjustedStepInterval && characterAnimationManager.velocityMagnitude > 0.1f)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        RaycastHit hit;
        AudioClip defaultClip = null;

        if (Physics.Raycast(footstepAudioSource.transform.position, Vector3.down, out hit, 1.25f))
        {
            foreach (var data in materialDataList)
            {
                if (data.materialName == "Concrete")
                {
                    defaultClip = data.footstepSounds[Random.Range(0, data.footstepSounds.Count)];
                }

                if (hit.collider.CompareTag(data.materialName))
                {
                    int randomIndex = Random.Range(0, data.footstepSounds.Count);

                    footstepAudioSource.pitch = Random.Range(minPitch, maxPitch);
                    footstepAudioSource.PlayOneShot(data.footstepSounds[randomIndex]);
                    return;
                }
            }

            if (defaultClip != null)
            {
                footstepAudioSource.pitch = Random.Range(minPitch, maxPitch);
                footstepAudioSource.PlayOneShot(defaultClip);
            }
        }
    }
}
