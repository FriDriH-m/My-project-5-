using UnityEngine;
using System.Collections;

public class IdleStateB : BaseStateBoss
{
    public override void EnterState(BossStateManager manager)
    {
        Debug.Log("idle");
        manager.StartCoroutine(Switch(manager));
        manager.SetSpeed(0);
    }
    public override void ExitState(BossStateManager manager) {}
    public override void UpdateState(BossStateManager manager)
    {
        //Debug.Log(manager.CheckDistance() + " - " + manager.agroDistance);
        if (manager.CheckDistance() < manager.agroDistance && manager.canSwitchState) manager.SwitchState(manager.agroState);
    }
    public IEnumerator Switch(BossStateManager manager)
    {
        manager.canSwitchState = false;
        yield return new WaitForSeconds(2);
        manager.canSwitchState = true;
    }
}
