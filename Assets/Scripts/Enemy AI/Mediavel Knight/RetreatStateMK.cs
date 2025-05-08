using GLTFast.Schema;
using UnityEngine;

public class RetreatState : BaseState
{
    float time = 0; // таймер
    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        //Debug.Log("retreat");
        manager.animator.SetBool("IsRetreat", true);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        manager.animator.SetBool("IsRetreat", false);
    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        time += Time.deltaTime;
        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);
        if (time >= 0.3f)
        {
            zoneManager.AttackAnimation();
            time = 0;
        }


        if (manager.CheckDistance() >= manager.attackDistance) manager.SwitchState(manager.attackState);
        if (zoneManager.defenseSide != "") manager.SwitchState(manager.defenceState);

        manager.StartRetreatMove();
    }
}
