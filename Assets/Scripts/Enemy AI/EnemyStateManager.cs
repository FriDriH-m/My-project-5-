using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float agroDistance;
    [SerializeField] public float attackDistance;
    private ZoneTriggerManager zoneManager;
    Transform target; // Цель преследования

    BaseState currentState;
    public IdleState idleState = new();
    public DefenceState defenceState = new();
    public AttackState attackState = new();
    public AgroState agroState = new();

    public void SwitchState(BaseState newState) // Метод, задающий текущее состояние
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }
    public void SetSpeed(float speed) => navMeshAgent.speed = speed;

    public void SetTarget(Transform newDestination) => target = newDestination;

    public float CheckDistance()
    {
        return (transform.position - target.transform.position).magnitude;
    }
    private void Start()
    {
        SwitchState(idleState); // Задается стандартное состояние
    }

    private void Update()
    {
        SetTarget(player); // Задает значение target Transform player
        navMeshAgent.destination = target.position; // Постоянно обновляет позицию
        currentState.UpdateState(this); // Вызывается метод Updatestate() текущего состояния
    }
}
