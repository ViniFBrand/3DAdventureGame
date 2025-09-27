using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    private PlayerState _currentState;

    public void ChangeState(PlayerState newState)
    {
        if (_currentState != null) _currentState.OnStateExit(this);
        _currentState = newState;
        _currentState.OnStateEnter(this);
    }
}
