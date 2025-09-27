using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public override void OnStateEnter(Player player)
    {
        base.OnStateEnter(player);
    }

    public override void HandleInput(Player player)
    {

    }

    public override void OnStateUpdate(Player player)
    {
        if (player.rb.velocity.magnitude == 0)
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
