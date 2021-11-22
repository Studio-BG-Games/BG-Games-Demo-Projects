using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class SuperPowerState : State
{
    public SuperPowerState(GameObject player, StateMachine stateMachine, GameController gameController): 
        base(player, stateMachine, gameController)
    {

    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("SuperPowerEnter");
        player.GetComponent<SpriteRenderer>().sprite = player.GetComponent<Player>().superPowerSkin;
        player.GetComponent<Player>().SetTrailToSuperPower();
        gameController.isSuperPowerState = true;
        Timing.RunCoroutine(Duration(), "duration");
    }

    public override void clickZone()
    {
        base.clickZone();
        gameController._currentBorder.GetComponents<PolygonCollider2D>()[1].enabled = false;
        var border = gameController._currentBorder;
        Timing.RunCoroutine(gameController._currentBorder.GetComponent<Border>().Destroy().CancelWith(border));
    }

    public IEnumerator<float> Duration()
    {
        yield return Timing.WaitForSeconds(Random.Range(8,15));
        gameController.stateMachine.ChangeState(gameController.normalState);
        yield break;
    }
    public override void Exit()
    {
        base.Exit();
        Timing.KillCoroutines("duration");
        gameController.isSuperPowerState = false;
        player.GetComponent<Player>().SetTrailToNormal();
        player.GetComponent<SpriteRenderer>().sprite = player.GetComponent<Player>().normalSkin;
    }
}


