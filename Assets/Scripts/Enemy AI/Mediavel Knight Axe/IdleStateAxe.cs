using UnityEngine;

public class IdleStateAxe : BaseStateAxe
{
    public override void EnterState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {

    }
    public override void UpdateState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        if (manager.CheckDistance() < manager.agroDistance) manager.SwitchState(manager.agroState);
    }
}
