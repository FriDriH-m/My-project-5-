using GLTFast.Schema;
using UnityEngine;

public class RetreatState : BaseState
{
    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        //Debug.Log("retreat");
        manager.animator.SetBool("IsRetreat", true);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        manager.animator.SetBool("IsRetreat", false);
    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);

        if (manager.CheckDistance() >= manager.attackDistance) manager.SwitchState(manager.attackState);
        if (zoneManager.defenseSide != "") manager.SwitchState(manager.defenceState);

        manager.StartRetreatMove();
    }
}
