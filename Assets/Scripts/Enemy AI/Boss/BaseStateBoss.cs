using UnityEngine;

public abstract class BaseStateBoss
{
    public abstract void EnterState(BossStateManager manager);
    public abstract void ExitState(BossStateManager manager);
    public abstract void UpdateState(BossStateManager manager);

}
