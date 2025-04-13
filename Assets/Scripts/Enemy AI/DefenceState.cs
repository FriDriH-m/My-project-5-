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
    }
}
