using UnityEngine;

public class IdleState : BaseState
{
    [SerializeField] GameObject player;
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManager manager)
    {

    }
    public override void UpdateState(EnemyStateManager manager)
    {
        if (manager.CheckDistance() < manager.agroDistance) manager.SwitchState(manager.agroState);
    }
}
