using UnityEngine;

public class AttackState : BaseState
{
    float time = 0; // ������
    float time1 = 0; // ������ ��� ����������� strafe

    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        //Debug.Log("attack");
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
        if (time > 0.3f) // ������ 0.3 ������� �������������� ��� �����
        {
            if ((manager.CheckDistance() > manager.attackDistance) && !manager.isAttacking) manager.SwitchState(manager.agroState);
            if (manager.CheckDistance() < manager.attackDistance - 1.2f) manager.SwitchState(manager.retreatState);
            zoneManager.AttackAnimation();
            time = 0;
        }
        time1 += Time.deltaTime;
        if (time1 > 2f)
        {
            manager.isAnimationDown = false; // ����� ��� ������� ����� ������� ����� ����� ������������� strafe
            time1 = 0;
        }

        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);

        zoneManager.StrafeAnimation();

        if (zoneManager.defenseSide != "") manager.SwitchState(manager.defenceState);
    }
}
