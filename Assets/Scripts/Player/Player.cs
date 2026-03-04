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

    public PlayerAbilityShoot playerAbilityShoot;

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    [Header("Flash")]
    public List<FlashColor> flashColors;

    [Header("Life")]
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
        if(playerAbilityShoot == null) playerAbilityShoot = GetComponent<PlayerAbilityShoot>();
    }

    private void Awake()
    {
        OnValidate();

        healthBase.OnDamage += Damage;
        healthBase.OnKill += OnKill;
    }
    void Update()
    {
        //Checks if Player is alive to turn on or off their rotation
        if(_alive)
            transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        else
            transform.Rotate(0,0,0);

        //Player Movement
        var inputAxisVertical = Input.GetAxis("Vertical");
        var speedVector = transform.forward * inputAxisVertical * speed;

        //Jump Mechanic
        if (characterController.isGrounded)
        {
            _vSpeed = 0f;
            if (Input.GetKeyDown(jumpKeyCode))
            {
                _vSpeed = jumpSpeed;
            }
        }

        //Application of gravity to Player
        _vSpeed -= gravity * Time.deltaTime;
        speedVector.y = _vSpeed;

        //Run Mechanic
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
            //Turn off Player movement and colliders on kill
            characterController.enabled = false; 
            colliders.ForEach(i => i.enabled = false);
            playerAbilityShoot.enabled = false;

            Invoke(nameof(Revive), 3f);
        }
    }

    private void Revive()
    {
        _alive = true;
        healthBase.ResetLife();
        animator.SetTrigger("revive");
        Respawn();
        healthBase.UpdateUI();
        //Turn on Player movement and colliders on revive
        characterController.enabled = true; 
        colliders.ForEach(i => i.enabled = true);
        playerAbilityShoot.enabled = true;
    }
    
    
    public void Damage(HealthBase h)
    {
        flashColors.ForEach(i => i.Flash());
        EffectsManager.Instance.ChangeVignette();
        ShakeCamera.Instance.Shake(5, 3, .2f);
    }

    public void Damage(float damage, Vector3 dir)
    {
        //Damage(damage);
    }

    #endregion

    [NaughtyAttributes.Button]
    public void Respawn()
    {
        if(CheckpointManager.Instance.HasCheckpoint())
        {
            transform.position = CheckpointManager.Instance.GetPositionFromLastCheckpoint();
        }
    }
}
