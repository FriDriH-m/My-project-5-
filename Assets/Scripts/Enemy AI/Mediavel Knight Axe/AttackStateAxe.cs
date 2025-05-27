using UnityEngine;

public class AttackStateAxe : BaseStateAxe
{
    float time = 0; // таймер
    float time1 = 0; // таймер для ограничения strafe

    public override void EnterState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        //Debug.Log("attack");
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        manager.animator.SetBool("StrafeR", false);
        manager.animator.SetBool("StrafeL", false);
    }
    public override void UpdateState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        time += Time.deltaTime;
        if (time > 0.3f) // каждые 0.5 секунды обрабатывается эта ветка
        {
            if ((manager.CheckDistance() > manager.attackDistance) && !manager.isAttacking) manager.SwitchState(manager.agroState);
            if (manager.CheckDistance() < manager.attackDistance - 0.8f) manager.SwitchState(manager.retreatState);
            zoneManager.AttackAnimation();
            time = 0;
        }
       
        
        time1 += Time.deltaTime;
        if (time1 > 2f)
        {
            time1 = 0;
        }

        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);

        zoneManager.StrafeAnimation();

        if (zoneManager.defenseSide == "") manager.SwitchState(manager.defenceState);
    }
}
