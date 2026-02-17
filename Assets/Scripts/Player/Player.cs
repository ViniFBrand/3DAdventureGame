using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour//, IDamageable
{
    public List<Collider> colliders;
    public Animator animator;

    [Header("Movement")]
    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = -9.8f;
    public float jumpSpeed = 15f;

    private float _vSpeed = 0f;

    public KeyCode jumpKeyCode = KeyCode.Space;

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    [Header("Flash")]
    public List<FlashColor> flashColors;

    public HealthBase healthBase;

    private bool _alive = true;

    #region EXERCISE MODULO 28 
    public Rigidbody rb;
    private PlayerState _currentState;

    public void ChangeState(PlayerState newState)
    {
        if (_currentState != null) _currentState.OnStateExit(this);
        _currentState = newState;
        _currentState.OnStateEnter(this);
    }
    #endregion

    private void OnValidate()
    {
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
    }

    private void Awake()
    {
        OnValidate();

        healthBase.OnDamage += Damage;
        healthBase.OnDamage += OnKill;
    }
    void Update()
    {
    //Player Movement
        if(_alive)
            transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        else
            transform.Rotate(0,0,0);

        var inputAxisVertical = Input.GetAxis("Vertical");
        var speedVector = transform.forward * inputAxisVertical * speed;

        if (characterController.isGrounded)
        {
            _vSpeed = 0f;
            if (Input.GetKeyDown(jumpKeyCode))
            {
                _vSpeed = jumpSpeed;
            }
        }


        _vSpeed -= gravity * Time.deltaTime;
        speedVector.y = _vSpeed;

        var isWalking = inputAxisVertical != 0;
        if (isWalking)
        {
            if(Input.GetKey(keyRun))
            {
                speedVector *= speedRun;
                animator.speed = speedRun;
            }
            else
            {
                animator.speed = 1;
            }
        }

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
    

    #region LIFE
    private void OnKill(HealthBase h)
    {
        if (_alive)
        {
            _alive = false;
            animator.SetTrigger("death");
            characterController.enabled = false;
            colliders.ForEach(i => i.enabled = false);
        }
    }
    
    public void Damage(HealthBase h)
    {
        flashColors.ForEach(i => i.Flash());
    }

    public void Damage(float damage, Vector3 dir)
    {
        //Damage(damage);
    }

    #endregion
}
