using UnityEngine;

public class AttackStateG : BaseStateG
{
    float time = 0; // таймер
    public override void EnterState(EnemyStateManagerG manager)
    {
        manager.isAnimationIdle = true;
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManagerG manager)
    {

    }
    public override void UpdateState(EnemyStateManagerG manager)
    {
        time += Time.deltaTime;
        if (time > 0.5f) // каждые 0.5 секунды обрабатывается эта ветка
        {
            if (manager.CheckDistance() > manager.attackDistance && !manager.isAttacking) manager.SwitchState(manager.agroState);
            manager.RandomAttack();
            time = 0;
        }
        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);   
    }
}
