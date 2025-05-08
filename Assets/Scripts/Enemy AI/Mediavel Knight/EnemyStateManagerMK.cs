using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public NavMeshAgent navMeshAgent; 
    public Transform player; // Transform игрока
    public Transform enemy; // Transform врага
    public float walkSpeed; // —корость ходьбы врага
    public float agroDistance; // ƒистанци€ агра врага
    public float attackDistance; // ƒистанци€ атаки врага
    public float angleSpeed = 28f; // скорость Strafe врага
    public Animator animator; // јниматор врага
    public float retreatSpeed = 1f; // ƒистанци€ Strafe врага

    public bool isAnimation = false; // ѕеременна€ дл€ рандомного Strafe.  огда она false, рандомно выбираетс€ следующа€ сторона Strafe
    public bool isAnimationDown = false; // ѕеременна€, чтобы враг после уклонени€ задев зону down не стрейфил
    public bool isAttacking = false; // если true, другие анимации не могут

    public Vector3 vectorToPlayer; // ¬ектор от врага к игроку
    public Vector3 enemyForward; // ¬ектор направлени€ взгл€да врага
    ZoneTriggerManager zoneManager; // ћенеджер зон, который отвечает за то, в какой зоне по€вилс€ меч игрока
    Transform target; // ÷ель преследовани€

    BaseState currentState;
    public IdleState idleState = new();
    public DefenceState defenceState = new();
    public AttackState attackState = new();
    public AgroState agroState = new();
    public RetreatState retreatState = new();

    public void SwitchState(BaseState newState) // ћетод, задающий текущее состо€ние
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

    public void EndAnimationStrafe() //ставит флаг если анимаци€ заврешилась (Strafe анимации), используетс€ Animation event
    {
        isAnimation = false;
    }

    public void StartAnimationDown() // срабатывает когда анимаци€ при задевании зоны down начинаетс€. Animation event
    {
        isAnimationDown = true; 
    }
    public void EndAnimationDown() // срабатывает когда анимаци€ при задевании зоны down заканчиваетс€. Animation event
    {
        zoneManager.defenseSide = "";
        isAnimationDown = false;
    }

    public void StartRetreatMove()
    {
        if (!isAttacking)
        {
            enemy.position = Vector3.Lerp(enemy.position, enemy.position - enemy.forward * 2f, retreatSpeed * Time.deltaTime); // плавно двигает врага на определенное рассто€ние
        }
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
        SwitchState(idleState); // «адаетс€ стандартное состо€ние
    }
    private void Update()
    {
        SetTarget(player); // «адает значение target Transform player
        navMeshAgent.destination = target.position; // ѕосто€нно обновл€ет позицию
        currentState.UpdateState(this, zoneManager); // ¬ызываетс€ метод Updatestate() текущего состо€ни€
    }
}
