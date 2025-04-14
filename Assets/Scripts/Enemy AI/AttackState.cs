using UnityEngine;

public class AttackState : BaseState
{
    float time = 0;
    bool leftMove = false;
    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        manager.isAnimationIdle = true;
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        manager.animator.SetBool("StrafeR", false);
        manager.animator.SetBool("StrafeL", false);
    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        time += Time.deltaTime;
        if (time > 0.5f) // каждые 0.2 секунды обрабатывается эта ветка
        {
            if (manager.CheckDistance() > manager.attackDistance) manager.SwitchState(manager.agroState);
            time = 0;
        }

        if (manager.CheckAngle() > 20f) manager.enemy.Rotate(0, -40f, 0);
        if (manager.CheckAngle() < -20f) manager.enemy.Rotate(0, 40f, 0);

        //if (!manager.isAnimation)
        //{
        //    int randInt = manager.RandInt();
        //    if (randInt == 1)
        //    {
        //        leftMove = true;
        //        manager.animator.SetBool("StrafeL", true);
        //        manager.animator.SetBool("StrafeR", false);
        //        manager.isAnimation = true;
        //    }
        //    else if (randInt == 2)
        //    {
        //        leftMove = false;
        //        manager.animator.SetBool("StrafeR", true);
        //        manager.animator.SetBool("StrafeL", false);
        //        manager.isAnimation = true;
        //    }
        //    else if (randInt == 0)
        //    {
        //        manager.animator.SetBool("StrafeR", false);
        //        manager.animator.SetBool("StrafeL", false);
        //        manager.isAnimation = true;
        //        manager.isAnimationIdle = true;
        //    }
        //}
        //if (manager.isAnimation && !manager.isAnimationIdle)
        //{
        //    if (leftMove)
        //    {
        //        manager.enemy.RotateAround(manager.player.position, Vector3.up, manager.angleSpeed * Time.deltaTime);
        //    }
        //    else manager.enemy.RotateAround(manager.player.position, Vector3.up, -1 * manager.angleSpeed * Time.deltaTime);
        //}

        if (zoneManager.summ != 0) manager.SwitchState(manager.defenceState);
    }
}
