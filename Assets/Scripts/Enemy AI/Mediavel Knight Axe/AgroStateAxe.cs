using GLTFast.Schema;
using UnityEngine;

public class AgroStateAxe : BaseStateAxe
{
    public override void EnterState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        Debug.Log("Agro");
        manager.animator.SetBool("IsWalking", true);
        manager.SetSpeed(manager.walkSpeed);
        
    }
    public override void ExitState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        manager.animator.SetBool("IsWalking", false);
    }
    public override void UpdateState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager)
    {
        if (zoneManager.defenseSide != "") manager.SwitchState(manager.defenceState);
        if (manager.CheckDistance() >= manager.agroDistance) manager.SwitchState(manager.idleState);
        if (manager.CheckDistance() <= manager.attackDistance) manager.SwitchState(manager.attackState);
    }
}
