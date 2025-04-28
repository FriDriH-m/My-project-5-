using UnityEngine;

public class RetreatStateAxe : BaseStateAxe
{
    public override void EnterState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        Debug.Log("retreat");
        manager.animator.SetBool("IsRetreat", true);
    }
    public override void ExitState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        manager.animator.SetBool("IsRetreat", false);
    }
    public override void UpdateState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);

        if (manager.CheckDistance() >= manager.attackDistance) manager.SwitchState(manager.attackState);
        if (zoneManager.top == 1 || zoneManager.down == 1) manager.SwitchState(manager.defenceState);

        manager.StartRetreatMove();
    }
}
