using GLTFast.Schema;
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
        manager.animator.SetBool("Walk Left", false);
        manager.animator.SetBool("Walk Right", false);
    }
    public override void UpdateState(BossStateManager manager)
    {
        time += Time.deltaTime;
        if (time > 0.5f) // каждые 0.3 секунды обрабатывается эта ветка
        {
            if ((manager.CheckDistance() > manager.attackDistance)) manager.SwitchState(manager.agroState);
            manager._strafingSide = Random.Range(0, 2);
            time = 0;
        }

        manager.Strafing();
        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);
    }
}
