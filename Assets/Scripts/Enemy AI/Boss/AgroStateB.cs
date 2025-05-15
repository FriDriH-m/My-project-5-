using System.Collections;
using UnityEngine;

public class AgroStateB : BaseStateBoss
{
    Coroutine _runCoroutine;
    public override void EnterState(BossStateManager manager)
    {
        //Debug.Log("agro");
        manager.animator.SetBool("Walk", true);
        manager.SetSpeed(manager.walkSpeed);
        _runCoroutine = manager.StartCoroutine(Running(manager));
    }
    public override void ExitState(BossStateManager manager)
    {
        manager.animator.SetBool("Walk", false);
        manager.animator.SetBool("Run", false);
        manager.StopCoroutine(_runCoroutine);
    }
    public override void UpdateState(BossStateManager manager)
    {
        if (manager.CheckDistance() >= manager.agroDistance) manager.SwitchState(manager.idleState);
        if (manager.CheckDistance() <= manager.attackDistance) manager.SwitchState(manager.attackState);
        
        
    }
    public IEnumerator Running(BossStateManager manager)
    {        
        yield return new WaitForSeconds(2f);
        manager.SetSpeed(manager.walkSpeed + 4);
        manager.animator.SetBool("Run", true);        
    }
}
