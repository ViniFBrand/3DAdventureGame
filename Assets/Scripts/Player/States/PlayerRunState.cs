using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.StateMachine;

public class PlayerRunState : PlayerState
{
    public override void OnStateEnter(Player player)
    {
        player.animator.SetTrigger("run");
    }

    public override void HandleInput(Player player)
    {
        if (Input.GetKeyDown(KeyCode.Space))    
        {
            player.ChangeState(new PlayerJumpState());
        }   
    }

    public override void OnStateUpdate(Player player)
    {
        if(player.rb.velocity.magnitude == 0)
        {
            player.ChangeState(new PlayerIdleState());
        }
    }

    public override void OnStateStay()
    {
        base.OnStateStay();
    }

    public override void OnStateExit(Player player)
    {
        player.animator.ResetTrigger("run");
    }
}
