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
        if (time > 0.2f) // каждые 0.2 секунды обрабатывается эта ветка
        {
            if (manager.CheckDistance() > manager.attackDistance) manager.SwitchState(manager.agroState);
            time = 0;
        }
        if (manager.CheckAngle() > 40f) manager.enemy.Rotate(0, 40 * Time.deltaTime * 10f, 0);
        //Debug.Log(manager.vectorToPlayer);
        Debug.Log(manager.CheckAngle());
        if (zoneManager.summ != 0) manager.SwitchState(manager.defenceState);
    }
}
