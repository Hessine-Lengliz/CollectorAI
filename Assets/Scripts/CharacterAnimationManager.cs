using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
    public Animator animator;
    public CharacterController characterController;
    public float velocityMagnitude;
    public float moveSpeed;
    public Transform characterModel;
    public float rotationSpeed;

    private float moveX;
    private float moveZ;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private float fallSpeed;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Time.timeScale = 4f;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Time.timeScale = 1f;
        } 

        moveX = 0f;
        moveZ = 0f;
        UpdateAnimatorParameters();
    }

    private void FixedUpdate()
    {
        if (!characterController)
        {
            return;
        }

        if (characterController.isGrounded)
        {
            fallSpeed = 0f;
        }
        else
        {
            fallSpeed += gravity * Time.fixedDeltaTime;
        }

        Vector3 direction = new Vector3(moveX, 0, moveZ);

        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        velocity = new Vector3(direction.x, fallSpeed, direction.z);
        characterController.Move(velocity * Time.fixedDeltaTime * moveSpeed);

        if (moveX != 0 || moveZ != 0)
        {
            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                characterModel.rotation = Quaternion.Slerp(characterModel.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            }
        }
    }

    void UpdateAnimatorParameters()
    {
        bool isIdle = Mathf.Approximately(moveX, 0f) && Mathf.Approximately(moveZ, 0f);
        //animator.SetBool("isIdle", isIdle);
        //animator.SetBool("isWalkingForward", !isIdle);
        animator.SetFloat("WalkSpeed", moveSpeed);
    }
}
