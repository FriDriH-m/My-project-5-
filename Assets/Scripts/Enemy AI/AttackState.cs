using GLTFast.Schema;
using UnityEngine;

public class AttackState : BaseState
{
    float time = 0;
    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {

    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        time += Time.deltaTime;
        if (time > 0.3f) // каждые 0.2 секунды обрабатывается эта ветка
        {
           
            if (manager.CheckDistance() > manager.attackDistance) manager.SwitchState(manager.agroState);
            time = 0;
        }

        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2, 0);

        if (manager.IsAnima)
        {
            if (manager.RandInt() == 0)
            {
                manager.animator.SetBool("StrafeL", true);
                manager.animator.SetBool("StrafeR", false);
                manager.enemy.position += -manager.enemy.right;
                manager.IsAnima = false;                
            }
            else
            {
                manager.animator.SetBool("StrafeR", true);
                manager.animator.SetBool("StrafeL", false);
                manager.enemy.position += manager.enemy.right;
                manager.IsAnima = false;
            }
        }

        if (zoneManager.summ != 0) manager.SwitchState(manager.defenceState);
    }
}
