using UnityEngine;

public class AgroState : BaseState
{
    ZoneTriggerManager zoneManager;

    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(manager.walkSpeed);
    }
    public override void ExitState(EnemyStateManager manager)
    {

    }
    public override void UpdateState(EnemyStateManager manager)
    {
        if (zoneManager != null) { Debug.Log("null"); }
        if (zoneManager.summ != 0) manager.SwitchState(manager.defenceState);
        if (manager.CheckDistance() >= manager.agroDistance) manager.SwitchState(manager.idleState);
        if (manager.CheckDistance() <= manager.attackDistance) manager.SwitchState(manager.attackState);
    }
}
