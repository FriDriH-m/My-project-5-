using UnityEngine;

public class IdleStateB : BaseStateBoss
{
    public override void EnterState(BossStateManager manager)
    {
        manager.SetSpeed(0);
    }
    public override void ExitState(BossStateManager manager) {}
    public override void UpdateState(BossStateManager manager)
    {
        if (manager.CheckDistance() < manager.agroDistance) manager.SwitchState(manager.agroState);
    }
}
