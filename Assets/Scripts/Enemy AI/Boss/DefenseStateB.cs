using UnityEngine;

public class DefenseStateB : BaseStateBoss
{
    public override void EnterState(BossStateManager manager)
    {
        //Debug.Log("idle");
        manager.SetSpeed(0);
        manager.animator.SetBool("Jump Back", true);
        _coroutine = manager.StartCoroutine(Switch(manager));
    }
    public override void ExitState(BossStateManager manager)
    {

    }
    public override void UpdateState(BossStateManager manager)
    {
        if (manager.CheckDistance() < manager.agroDistance) manager.SwitchState(manager.agroState);
    }
}
