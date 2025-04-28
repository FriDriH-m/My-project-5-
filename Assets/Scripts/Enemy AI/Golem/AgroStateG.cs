using UnityEngine;

public class AgroStateG : BaseStateG
{
    public override void EnterState(EnemyStateManagerG manager)
    {
        manager.animator.SetBool("IsWalking", true);
        manager.SetSpeed(manager.walkSpeed);
    }
    public override void ExitState(EnemyStateManagerG manager)
    {
        manager.animator.SetBool("IsWalking", false);
    }
    public override void UpdateState(EnemyStateManagerG manager)
    {
        if (manager.CheckDistance() >= manager.agroDistance) manager.SwitchState(manager.idleState);
        if (manager.CheckDistance() <= manager.attackDistance) manager.SwitchState(manager.attackState);
    }
}
