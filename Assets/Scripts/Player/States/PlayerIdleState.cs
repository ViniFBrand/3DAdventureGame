using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.StateMachine;

public class PlayerIdleState : PlayerState
{
    public override void OnStateEnter(Player player)
    {
        player.animator.SetTrigger("idle");
    }

    public override void HandleInput(Player player)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            player.ChangeState(new PlayerRunState());
        }
    }

    public override void OnStateStay()
    {
        base.OnStateStay();
    }

    public override void OnStateExit(Player player)
    {
        player.animator.ResetTrigger("idle");
    }
}
