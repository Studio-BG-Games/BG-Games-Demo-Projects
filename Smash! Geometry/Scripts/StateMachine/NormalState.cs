using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class NormalState : State
{
    public NormalState(GameObject player, StateMachine stateMachine, GameController gameController) :
        base(player, stateMachine, gameController)
    {

    }
    public override void Enter()
    {
        Debug.Log("NormalEnter");
        base.Enter();
    }

    public override void clickZone()
    {
        base.clickZone();
    }
    public override void Exit()
    {
        base.Exit();
    }
}