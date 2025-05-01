using UnityEngine;

public class AgroState : BaseState
{

    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        manager.animator.SetBool("IsWalking", true);
        manager.SetSpeed(manager.walkSpeed);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        manager.animator.SetBool("IsWalking", false);
    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        if (zoneManager == null) Debug.Log("нулл блядь");
        if (zoneManager.top == 1 || zoneManager.left == 1 || zoneManager.right == 1 || zoneManager.down == 1)
        {
            manager.SwitchState(manager.defenceState);
        }

        if (manager.CheckDistance() >= manager.agroDistance) manager.SwitchState(manager.idleState);
        if (manager.CheckDistance() <= manager.attackDistance) manager.SwitchState(manager.attackState);
    }
}
