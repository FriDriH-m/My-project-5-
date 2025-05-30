using UnityEngine;

public class AttackStateAxe : BaseStateAxe
{
    float time = 0; // таймер

    public override void EnterState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        Debug.Log("attack");
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        zoneManager.strafing = false;
        manager.animator.SetBool("StrafeL", false);
        manager.animator.SetBool("StrafeR", false);
    }
    public override void UpdateState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        time += Time.deltaTime;
        if (time > 0.3f) // каждые 0.5 секунды обрабатывается эта ветка
        {
            if ((manager.CheckDistance() > manager.attackDistance) && !manager.isAttacking) manager.SwitchState(manager.agroState);
            if (manager.CheckDistance() < manager.attackDistance - 0.8f && !manager.isAttacking) manager.SwitchState(manager.retreatState);
            zoneManager.AttackAnimation();
            time = 0;
        }

        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);
        zoneManager.StrafeAnimation();

        if (zoneManager.defenseSide != "") manager.SwitchState(manager.defenceState);
    }
}
