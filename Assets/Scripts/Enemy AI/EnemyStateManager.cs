using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] public Transform player;
    [SerializeField] public Transform enemy;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float agroDistance;
    [SerializeField] public float attackDistance;
    [SerializeField] public Animator animator;
    public bool IsAnima = true;
    public Vector3 vectorToPlayer;
    public Vector3 enemyForward;
    ZoneTriggerManager zoneManager; // Менеджер зон, который отвечает за то, в какой зоне появился меч игрока
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
            currentState.ExitState(this, zoneManager);
        }
        currentState = newState;
        currentState.EnterState(this, zoneManager);
    }
    public void SetSpeed(float speed) => navMeshAgent.speed = speed;

    public void SetTarget(Transform newDestination) => target = newDestination;

    public float CheckAngle() // проверяет угол между направлением взгляда врага и игрока
    {
        vectorToPlayer = (player.position - enemy.position);
        vectorToPlayer.y = 0;
        vectorToPlayer.Normalize();

        enemyForward = enemy.forward;
        enemyForward.y = 0;
        enemyForward.Normalize();
        return Vector3.SignedAngle(vectorToPlayer, enemyForward, Vector3.up);
    }

    public float CheckDistance()
    {
        return (transform.position - target.transform.position).magnitude;
    }

    public int RandInt()
    {
        return Random.Range(0, 2);
    }

    public void EndAnimation() //ставит флаг если анимация заврешилась (Strafe анимации)
    {
        IsAnima = true;
    }

    private void Start()
    {
        zoneManager = GetComponent<ZoneTriggerManager>();
        SwitchState(idleState); // Задается стандартное состояние
    }

    private void Update()
    {
        SetTarget(player); // Задает значение target Transform player
        navMeshAgent.destination = target.position; // Постоянно обновляет позицию
        currentState.UpdateState(this, zoneManager); // Вызывается метод Updatestate() текущего состояния
    }
}
