using UnityEngine;

public abstract class BaseStateAxe
{
    public abstract void EnterState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager);
    public abstract void ExitState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager);
    public abstract void UpdateState(EnemyStateManagerAxe manager, ZoneTriggerManagerAxe zoneManager);
}
