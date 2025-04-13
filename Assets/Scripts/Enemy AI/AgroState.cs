using UnityEngine;

public class AgroState : BaseState
{

    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        manager.SetSpeed(manager.walkSpeed);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {

    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {

        if (zoneManager.summ != 0) manager.SwitchState(manager.defenceState);
        if (manager.CheckDistance() >= manager.agroDistance) manager.SwitchState(manager.idleState);
        if (manager.CheckDistance() <= manager.attackDistance) manager.SwitchState(manager.attackState);
    }
}
