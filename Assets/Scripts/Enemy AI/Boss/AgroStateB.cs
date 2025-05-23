using System.Collections;
using UnityEngine;

public class AgroStateB : BaseStateBoss
{
    Coroutine _runCoroutine;
    public override void EnterState(BossStateManager manager)
    {
        manager.animator.SetBool("Walk", true);
        manager.SetSpeed(manager.walkSpeed);
        _runCoroutine = manager.StartCoroutine(Running(manager));

    }
    public override void ExitState(BossStateManager manager)
    {
        manager.animator.SetBool("Walk", false);
        manager.animator.SetBool("Run", false);
        if (_runCoroutine != null) manager.StopCoroutine(_runCoroutine);
        _runCoroutine = null;
    }
    public override void UpdateState(BossStateManager manager)
    {
        if (manager.CheckDistance() >= manager.agroDistance && manager.canSwitchState) manager.SwitchState(manager.idleState);
        if (manager.CheckDistance() <= manager.attackDistance && manager.canSwitchState) manager.SwitchState(manager.attackState);

        manager.FastDistanceAttack(_runCoroutine); // включает атаку с быстрым сближением на расстоянии attackDistance + 3
        manager.GetCloser(true); // само сближение до расстояния 3
    }
    public IEnumerator Running(BossStateManager manager)
    {
        yield return new WaitForSeconds(2f);
        manager.SetSpeed(manager.walkSpeed + 4);
        manager.animator.SetBool("Run", true);
    }
}
