using UnityEngine;

public class IdleStateG : BaseStateG
{
    public override void EnterState(EnemyStateManagerG manager)
    {
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManagerG manager) { }
    public override void UpdateState(EnemyStateManagerG manager)
    {
        if (manager.CheckDistance() < manager.agroDistance) manager.SwitchState(manager.agroState);
    }
}
