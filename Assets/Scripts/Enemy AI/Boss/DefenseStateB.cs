using UnityEngine;

using System.Collections;

public class DefenseStateB : BaseStateBoss
{
    Coroutine _coroutine;
    public override void EnterState(BossStateManager manager)
    {
        manager.SetSpeed(0);
        Debug.Log("Defense");
        manager.animator.SetBool("Jump Back", true);
        _coroutine = manager.StartCoroutine(Switch(manager));
    }
    public override void ExitState(BossStateManager manager)
    {
        manager.animator.SetBool("Jump Back", false);
        manager.StopCoroutine(_coroutine);
        _coroutine = null;
    }
    public override void UpdateState(BossStateManager manager)
    {
        manager.JumpBack();
        if (manager.CheckDistance() > manager.attackDistance && manager.canSwitchState) manager.SwitchState(manager.agroState);
        if (manager.CheckDistance() < manager.attackDistance && manager.canSwitchState) manager.SwitchState(manager.attackState);
    }
    public IEnumerator Switch(BossStateManager manager)
    {
        manager.canSwitchState = false;
        yield return new WaitForSeconds(1);
        manager.canSwitchState = true;
    }
}
