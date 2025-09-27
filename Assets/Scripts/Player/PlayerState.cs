using Ebac.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : StateBase
{
    public virtual void OnStateEnter(Player player)
    {
        Debug.Log("OnStateEnter");
    }
    public virtual void OnStateStay(Player player)
    {
        Debug.Log("OnStateStay");
    }
    public virtual void OnStateUpdate(Player player)
    {
        Debug.Log("OnStateUpdate");
    }
    public virtual void HandleInput(Player player)
    {
        Debug.Log("Input");
    }
    public virtual void OnStateExit(Player player)
    {
        Debug.Log("OnStateExit");
    }
}
