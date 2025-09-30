using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;

    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = -9.8f;
    public float jumpSpeed = 15f;

    private float vSpeed = 0f;

    public KeyCode jumpKeyCode = KeyCode.Space;

    #region EXERCISE
    public Rigidbody rb;
    private PlayerState _currentState;

    public void ChangeState(PlayerState newState)
    {
        if (_currentState != null) _currentState.OnStateExit(this);
        _currentState = newState;
        _currentState.OnStateEnter(this);
    }
    #endregion

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);

        var inputAxisVertical = Input.GetAxis("Vertical");
        var speedVector = transform.forward * inputAxisVertical * speed;

        if (characterController.isGrounded)
        {
            vSpeed = 0f;
            if (Input.GetKeyDown(jumpKeyCode))
            {
                vSpeed = jumpSpeed;
            }
        }


        vSpeed -= gravity * Time.deltaTime;
        speedVector.y = vSpeed;

        characterController.Move(speedVector * Time.deltaTime);

        animator.SetBool("run", inputAxisVertical != 0);

        /* if(inputAxisVertical != 0)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }*/
    }
}
