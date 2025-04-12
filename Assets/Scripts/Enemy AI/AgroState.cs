using UnityEngine;

public class AgroState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(manager.walkSpeed);
    }
    public override void ExitState(EnemyStateManager manager)
    {

    }
    public override void UpdateState(EnemyStateManager manager)
    {
        if (manager.CheckDistance() >= manager.agroDistance) manager.SwitchState(manager.idleState);
        if (manager.CheckDistance() <= manager.attackDistance) manager.SwitchState(manager.attackState);
    }
}
