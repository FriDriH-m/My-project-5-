using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AgroStateB : BaseStateBoss
{
    public override void EnterState(BossStateManager manager)
    {
        manager.animator.SetBool("Walk", true);
        manager.SetSpeed(manager.walkSpeed);
        manager._runCoroutine = manager.StartCoroutine(Running(manager));
        //manager.BossSound.AgroStateActive();
        Debug.Log("Агро");
    }
    public override void ExitState(BossStateManager manager)
    {
        manager.animator.SetBool("Walk", false);        
    }
    public override void UpdateState(BossStateManager manager)
    {
        //Debug.Log(manager.CheckDistance() + " - " + manager.agroDistance);
        if (manager.CheckDistance() >= manager.agroDistance && manager.canSwitchState) manager.SwitchState(manager.idleState);
        if (manager.CheckDistance() <= manager.attackDistance && manager.canSwitchState) manager.SwitchState(manager.attackState);

        manager.FastDistanceAttack(manager._runCoroutine); // включает атаку с быстрым сближением на расстоянии attackDistance + 3
        manager.GetCloser(true); // само сближение до расстояния 3
    }
    public IEnumerator Running(BossStateManager manager)
    {
        yield return new WaitForSeconds(2f);
        manager.SetSpeed(manager.walkSpeed + 5);
        manager.animator.SetBool("Run", true);
    }
}
