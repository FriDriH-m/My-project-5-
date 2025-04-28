using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {

    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        if (manager.CheckDistance() < manager.agroDistance) manager.SwitchState(manager.agroState);
    }
}
