using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DefenceState : BaseState
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
        if (zoneManager.summ == 0) manager.SwitchState(manager.attackState);

        if (zoneManager.top == 1) manager.animator.SetBool("top", true);
        else manager.animator.SetBool("top", false);

        if (zoneManager.down == 1) manager.animator.SetBool("down", true);
        else manager.animator.SetBool("down", false);

        if (zoneManager.left == 1) manager.animator.SetBool("left", true);
        else manager.animator.SetBool("left", false);

        if (zoneManager.right == 1) manager.animator.SetBool("right", true);
        else manager.animator.SetBool("right", false);

        if (zoneManager.middle == 1) manager.animator.SetBool("middle", true);
        else manager.animator.SetBool("middle", false);
    }
}
