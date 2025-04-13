using UnityEngine;

public class AttackState : BaseState
{
    ZoneTriggerManager zoneManager;

    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManager manager)
    {

    }
    public override void UpdateState(EnemyStateManager manager)
    {
        if (zoneManager.summ != 0) manager.SwitchState(manager.defenceState);
        if (manager.CheckDistance() > manager.attackDistance) manager.SwitchState(manager.agroState);
    }
}
