using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager);
    public abstract void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager);
    public abstract void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager);

}
