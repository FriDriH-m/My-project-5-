using UnityEngine;

public class AttackStateB : BaseStateBoss
{
    float time = 0; // таймер
    public override void EnterState(BossStateManager manager)
    {
        //Debug.Log("attack");
        manager.SetSpeed(0);
    }
    public override void ExitState(BossStateManager manager)
    {

    }
    public override void UpdateState(BossStateManager manager)
    {
        time += Time.deltaTime;
        if (time > 0.3f) // каждые 0.3 секунды обрабатывается эта ветка
        {
            if ((manager.CheckDistance() > manager.attackDistance)) manager.SwitchState(manager.agroState);       
            time = 0;
        }

        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);
    }
}
