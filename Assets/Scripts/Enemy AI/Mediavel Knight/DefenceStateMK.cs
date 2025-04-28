using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DefenceState : BaseState
{

    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        //Debug.Log("defense");
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {

    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        if (zoneManager.top == 0 && zoneManager.left == 0 && zoneManager.right == 0 && zoneManager.down == 0) manager.SwitchState(manager.attackState);
        if ((manager.CheckDistance() < manager.attackDistance - 1) && (zoneManager.top == 0 && zoneManager.left == 0 && zoneManager.right == 0 && zoneManager.down == 0)) manager.SwitchState(manager.retreatState);

        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);

        if (zoneManager.defenceTime >= 1.5f)
        {
            zoneManager.ResetZone("right"); //
            zoneManager.ResetZone("left");  // обнуляет все анимации защиты
            zoneManager.ResetZone("top");   //
            zoneManager.ResetZone("down");
            zoneManager.SetActiveZone("down"); // после обнуления других анимаций отступает
            zoneManager.defenceTime = 0;
        }

        zoneManager.DefenseAnimation();
    }
}
