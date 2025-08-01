using UnityEngine;

public class DefenseStateAxe : BaseStateAxe
{
    public override void EnterState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        //Debug.Log("defense");
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {

    }
    public override void UpdateState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        if (zoneManager.defenseSide == "") manager.SwitchState(manager.attackState);
        if ((manager.CheckDistance() < manager.attackDistance - 1) && zoneManager.defenseSide == "") manager.SwitchState(manager.retreatState);

        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);

        if (zoneManager.defenceTime >= 1.5f)
        {
            zoneManager.defenseSide = "down";
            zoneManager.defenceTime = 0;
        }

        zoneManager.DefenseAnimation();
    }
}
