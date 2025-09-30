using Ebac.Core;
using Ebac.Core.Sigleton;
using Ebac.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PlayerStateMachine : Singleton<PlayerStateMachine>
{
    public enum PlayerStates
    {
        IDLE,
        RUN,
        JUMP
    }

    public StateMachine<PlayerStates> stateMachine;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        stateMachine = new StateMachine<PlayerStates>();

        stateMachine.Init();
        stateMachine.RegisterStates(PlayerStates.IDLE, new PlayerIdleState());
        stateMachine.RegisterStates(PlayerStates.RUN, new PlayerRunState());
        stateMachine.RegisterStates(PlayerStates.JUMP, new PlayerJumpState());

        stateMachine.SwitchState(PlayerStates.IDLE);
    }
}
