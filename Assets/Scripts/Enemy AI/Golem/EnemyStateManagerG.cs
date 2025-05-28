using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManagerG : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    public Transform player;
    [SerializeField] public Transform enemy; 
    [SerializeField] public float walkSpeed; // —корость ходьбы врага
    [SerializeField] public float agroDistance; // ƒистанци€ агра врага
    [SerializeField] public float attackDistance; // ƒистанци€ атаки врага
    [SerializeField] public float angleSpeed = 28f; // скорость Strafe врага
    [SerializeField] public Animator animator; // јниматор врага
    [SerializeField] public float retreatSpeed = 1f; // —корость отступлени€ врага
    public Vector3 vectorToPlayer; // ¬ектор от врага к игроку
    public Vector3 enemyForward; // ¬ектор направлени€ взгл€да врага
    Transform target;
    public DamageCount damageCount;

    public bool isAnimationIdle = false; // ѕеременна€, чтобы враг не двигалс€, когда проигрываетс€ анимаци€ idle. »з-за рандомного Strafe
    public bool isAttacking = false; // если true, другие анимации не могут

    BaseStateG currentState;
    public IdleStateG idleState = new();
    public AgroStateG agroState = new();
    public AttackStateG attackState = new();

    private void Start()
    {
        damageCount = GetComponent<DamageCount>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SwitchState(idleState);
    }

    public void SwitchState(BaseStateG newState)
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
    public void ResetAttack() // в idle animation event который заканчивает анимации атаки
    {
        animator.SetBool("AttackLeft", false);
        animator.SetBool("AttackRight", false);
        damageCount.attacking = false;
        isAttacking = false;
    }
    public void RandomAttack() 
    {
        int randomInt = Random.Range(0, 2);
        int chanceOfAttack = Random.Range(0, 3);
        if (chanceOfAttack == 1)
        {
            if (randomInt == 0)
            {
                animator.SetBool("AttackLeft", true);
            }
            else
            {
                animator.SetBool("AttackRight", true);
            }
        }
    }
    public void StartAttackAnimation()
    {
        damageCount.attacking = true;
        isAttacking = true;
    }
    public void EndAttackAnimation()
    {
        damageCount.attacking = false;
        isAttacking = false;
    }
    private void Update()
    {
        SetTarget(player); // «адает значение target Transform player
        navMeshAgent.destination = target.position; // ѕосто€нно обновл€ет позицию
        currentState.UpdateState(this); // ¬ызываетс€ метод Updatestate() текущего состо€ни€
    }
}
