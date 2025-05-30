using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManagerAxe : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] public Transform player; // Transform игрока
    [SerializeField] public Transform enemy; // Transform врага
    [SerializeField] public float walkSpeed; // —корость ходьбы врага
    [SerializeField] public float agroDistance; // ƒистанци€ агра врага
    [SerializeField] public float attackDistance; // ƒистанци€ атаки врага
    [SerializeField] public float angleSpeed = 28f; // скорость Strafe врага
    [SerializeField] public Animator animator; // јниматор врага
    [SerializeField] public float retreatSpeed = 1f; // ƒистанци€ Strafe врага
    public DamageCount damageCount;

    public bool isAttacking = false; // если true, другие анимации не могут

    public Vector3 vectorToPlayer; // ¬ектор от врага к игроку
    public Vector3 enemyForward; // ¬ектор направлени€ взгл€да врага
    ZoneTriggerManagerAxe zoneManager; // ћенеджер зон, который отвечает за то, в какой зоне по€вилс€ меч игрока
    Transform target; // ÷ель преследовани€

    BaseStateAxe currentState;
    public IdleStateAxe idleState = new();
    public DefenseStateAxe defenceState = new();
    public AttackStateAxe attackState = new();
    public AgroStateAxe agroState = new();
    public RetreatStateAxe retreatState = new();

    public void SwitchState(BaseStateAxe newState) // ћетод, задающий текущее состо€ние
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

    public float CheckAngle() // провер€ет угол между направлением взгл€да врага и игрока
    {
        vectorToPlayer = (player.position - enemy.position);
        vectorToPlayer.y = 0;
        vectorToPlayer.Normalize();

        enemyForward = enemy.forward;
        enemyForward.y = 0;
        enemyForward.Normalize();
        return Vector3.SignedAngle(vectorToPlayer, enemyForward, Vector3.up);
    }
    public float CheckDistance() // провер€ет рассто€ние между врагом и игроком
    {
        return (transform.position - target.transform.position).magnitude;
    }

    public void StartAnimationDown() // срабатывает когда анимаци€ при задевании зоны down начинаетс€. Animation event
    {
        enemy.position = Vector3.MoveTowards(enemy.position, enemy.position - enemy.forward * 2f, 4f * Time.deltaTime);
    }
    public void StartRetreatMove()
    {
        if (!isAttacking)
        {
            enemy.position = Vector3.Lerp(enemy.position, enemy.position - enemy.forward * 2f, retreatSpeed * Time.deltaTime); // плавно двигает врага на определенное рассто€ние
        }
    }
    public void EndAnimationDown() // срабатывает когда анимаци€ при задевании зоны down заканчиваетс€. Animation event
    {
        zoneManager.defenseSide = "";
    }
    public void StartAttackAnimation()
    {
        zoneManager.strafing = false;
        animator.SetBool("StrafeL", false);
        animator.SetBool("StrafeR", false);
    }
    public void EndImpactAnimation()
    {
        animator.SetBool("HeadImpact", false);
        animator.SetBool("TorsoImpact", false);
    }
    private void Start()
    {
        damageCount = GetComponent<DamageCount>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        zoneManager = GetComponent<ZoneTriggerManagerAxe>();
        SwitchState(idleState); // «адаетс€ стандартное состо€ние
    }
    private void Update()
    {
        if (damageCount.revive)
        {
            SwitchState(idleState);
        }
        SetTarget(player); // «адает значение target Transform player
        navMeshAgent.destination = target.position; // ѕосто€нно обновл€ет позицию
        currentState.UpdateState(this, zoneManager); // ¬ызываетс€ метод Updatestate() текущего состо€ни€
    }
}
