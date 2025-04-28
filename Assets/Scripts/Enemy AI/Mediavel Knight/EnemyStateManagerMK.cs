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
    [SerializeField] public float retreatSpeed = 1f; // Дистанция Strafe врага

    public bool isAnimation = false; // Переменная для рандомного Strafe. Когда она false, рандомно выбирается следующая сторона Strafe
    public bool isAnimationIdle = false; // Переменная, чтобы враг не двигался, когда проигрывается анимация idle. Из-за рандомного Strafe
    public bool isAnimationDown = false; // Переменная, чтобы враг после уклонения задев зону down не стрейфил
    public bool isAttacking = false; // если true, другие анимации не могут

    public Vector3 vectorToPlayer; // Вектор от врага к игроку
    public Vector3 enemyForward; // Вектор направления взгляда врага
    ZoneTriggerManager zoneManager; // Менеджер зон, который отвечает за то, в какой зоне появился меч игрока
    Transform target; // Цель преследования

    BaseState currentState;
    public IdleState idleState = new();
    public DefenceState defenceState = new();
    public AttackState attackState = new();
    public AgroState agroState = new();
    public RetreatState retreatState = new();

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
    public float CheckDistance() // проверяет расстояние между врагом и игроком
    {
        return (transform.position - target.transform.position).magnitude;
    }

    public void EndAnimationStrafe() //ставит флаг если анимация заврешилась (Strafe анимации), используется Animation event
    {
        isAnimation = false;
        isAnimationIdle = false;
    }

    public void StartAnimationDown() // срабатывает когда анимация при задевании зоны down начинается. Animation event
    {
        if (!enemy.CompareTag("Shield"))
        {
            enemy.position = Vector3.MoveTowards(enemy.position, enemy.position - enemy.forward * 2f, 4f * Time.deltaTime); // плавно двигает врага на определенное расстояние
            // второе значение верхней строчки не имеет значения, главное чтобы не было слишком маленьким или равным нулю. 
            // все потому что оно проигрывается каждый кадр и дальность хода зависит от скорости передвижения, ведь анимация длится фикс-ое время и чем быстрее, тем дальше.
        }
        isAnimationDown = true; 
    }
    public void StartRetreatMove()
    {
        if (!isAttacking)
        {
            enemy.position = Vector3.Lerp(enemy.position, enemy.position - enemy.forward * 2f, retreatSpeed * Time.deltaTime); // плавно двигает врага на определенное расстояние
        }
    }
    public void EndAnimationDown() // срабатывает когда анимация при задевании зоны down заканчивается. Animation event
    {
        zoneManager.ResetZone("down");
        isAnimationDown = false;
    }
    public void StartAttackAnimation()
    {
        animator.SetBool("StrafeR", false);
        animator.SetBool("StrafeL", false);
        isAttacking = true;
    }
    public void EndAttackAnimation()
    {
        isAttacking = false;
    }
    public void EndImpactAnimation()
    {
        animator.SetBool("HeadImpact", false);
        animator.SetBool("TorsoImpact", false);

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
