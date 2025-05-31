using UnityEngine;
using System.Collections;


public class DefenceState : BaseState
{
    private bool isRetreating = false;
    private Coroutine _coroutine;
    private Coroutine _retreatCoroutine;
    public override void EnterState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        //Debug.Log("defense");
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        zoneManager.defenceTime = 0; // сбрасываем таймер при выходе из состо€ни€
    }
    public override void UpdateState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        if (zoneManager.defenseSide == "" && _coroutine == null)
        {
            _coroutine = manager.StartCoroutine(SwitchDefenseState(manager, zoneManager));
        } 
        else if (zoneManager.defenseSide != "" && _coroutine != null) { 
            manager.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        if ((manager.CheckDistance() < manager.attackDistance - 1) && (zoneManager.defenseSide == ""))
        {
            if (_coroutine != null)
            {
                manager.StopCoroutine(_coroutine);
                _coroutine = null;
            }
            manager.SwitchState(manager.retreatState);
            
        }
        if (manager.CheckAngle() > 1f) manager.enemy.Rotate(0, -2f, 0);
        if (manager.CheckAngle() < -1f) manager.enemy.Rotate(0, 2f, 0);
        
        zoneManager.defenceTime += Time.deltaTime;
        
        if (zoneManager.defenceTime >= 1f && !isRetreating && _retreatCoroutine == null)
        {
            _retreatCoroutine = manager.StartCoroutine(HandleRetreat(manager, zoneManager));
        }

        zoneManager.DefenseAnimation();

        if(manager.isAnimationDown)
        {
            if (!manager.enemy.CompareTag("Shield"))
            {
                manager.navMeshAgent.Move(-manager.enemy.forward * 4f * Time.deltaTime); // плавно двигает врага на определенное рассто€ние
            }
        }

    }
    private IEnumerator SwitchDefenseState(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        yield return new WaitForSeconds(0.1f);
        if (manager.CheckDistance() <= manager.attackDistance)
        {
            manager.SwitchState(manager.attackState);
        }
        if (manager.CheckDistance() > manager.attackDistance)
        {
            manager.SwitchState(manager.agroState);
        }
        _coroutine = null;
    }
    private IEnumerator HandleRetreat(EnemyStateManager manager, ZoneTriggerManager zoneManager)
    {
        isRetreating = true;
        zoneManager.StartRetreat();
        yield return new WaitForSeconds(1f);
        zoneManager.defenceTime = 0;
        isRetreating = false;
        _retreatCoroutine = null;
    }
}