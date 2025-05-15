using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class BossStateManager : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public Vector3 vectorToPlayer;
    public Vector3 enemyForward;
    public Transform player;
    public Transform enemy;
    public float walkSpeed;
    public float agroDistance; 
    public float attackDistance; 
    public float angleSpeed = 28f;
    public Animator animator;
    Transform target;

    BaseStateBoss currentState;    
    public IdleStateB idleState = new();
    public DefenseStateB defenseState = new();
    public AttackStateB attackState = new();
    public AgroStateB agroState = new();

    public void SwitchState(BaseStateBoss newState) 
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
    public float CheckAngle() 
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
    private void Start()
    {
        enemy = transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SwitchState(idleState); 
    }
    private void Update()
    {
        SetTarget(player); 
        navMeshAgent.destination = target.position; 
        currentState.UpdateState(this); 
    }
}
