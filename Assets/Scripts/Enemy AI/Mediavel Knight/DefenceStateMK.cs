using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class DefenceState : BaseState
{

    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        Debug.Log("defense");
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {

    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        if (zoneManager.defenseSide == "") manager.SwitchState(manager.attackState);
        if ((manager.CheckDistance() < manager.attackDistance - 1) && (zoneManager.defenseSide == "")) manager.SwitchState(manager.retreatState);

        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);

        if (zoneManager.defenceTime >= 1.5f)
        {
            zoneManager.defenseSide = "";
            zoneManager.defenseSide = "down"; // после обнуления других анимаций отступает
            zoneManager.defenceTime = 0;
        }

        zoneManager.DefenseAnimation();
        if(manager.isAnimationDown)
        {
            if (!manager.enemy.CompareTag("Shield"))
            {
                manager.navMeshAgent.Move(-manager.enemy.forward * 4f * Time.deltaTime); // плавно двигает врага на определенное расстояние
            }
        }
    }
}
