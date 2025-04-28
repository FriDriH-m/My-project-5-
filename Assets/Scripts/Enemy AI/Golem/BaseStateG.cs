using UnityEngine;

public abstract class BaseStateG
{
    public abstract void EnterState(EnemyStateManagerG manager);
    public abstract void ExitState(EnemyStateManagerG manager);
    public abstract void UpdateState(EnemyStateManagerG manager);
}
