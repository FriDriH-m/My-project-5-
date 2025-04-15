using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent; 
    [SerializeField] public Transform player; // Transform игрока
    [SerializeField] public Transform enemy; // Transform врага
    [SerializeField] public float walkSpeed; // Скорость ходьбы врага
    [SerializeField] public float agroDistance; // Дистанция агра врага
    [SerializeField] public float attackDistance; // Дистанция атаки врага
    [SerializeField] public float angleSpeed = 28f; // скорость Strafe врага
    [SerializeField] public Animator animator; // Аниматор врага

    public bool isAnimation = false; // Переменная для рандомного Strafe. Когда она false, рандомно выбирается следующая сторона Strafe
    public bool isAnimationIdle = false;
    public bool isAnimationDown = false;

    public Vector3 vectorToPlayer; // Вектор от врага к игроку
    public Vector3 enemyForward; // Вектор направления взгляда врага
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
        return Random.Range(0, 4);
    }

    public void EndAnimation() //ставит флаг если анимация заврешилась (Strafe анимации), используется Animation event
    {
        isAnimation = false;
        isAnimationIdle = false;
    }

    public void StartAnimationDown()
    {
        isAnimationDown = true;
        zoneManager.down = 0;
    }
    public void Move()
    {
        Vector3 backPosition = enemy.position - enemy.forward * 10f;
        enemy.position = Vector3.MoveTowards(enemy.position, backPosition, 10f * Time.deltaTime);
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
