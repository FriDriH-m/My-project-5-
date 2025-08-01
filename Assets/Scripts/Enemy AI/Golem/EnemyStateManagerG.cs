using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManagerG : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    public Transform player;
    [SerializeField] public Transform enemy; 
    [SerializeField] public float walkSpeed; // �������� ������ �����
    [SerializeField] public float agroDistance; // ��������� ���� �����
    [SerializeField] public float attackDistance; // ��������� ����� �����
    [SerializeField] public float angleSpeed = 28f; // �������� Strafe �����
    [SerializeField] public Animator animator; // �������� �����
    [SerializeField] public float retreatSpeed = 1f; // �������� ����������� �����
    public Vector3 vectorToPlayer; // ������ �� ����� � ������
    public Vector3 enemyForward; // ������ ����������� ������� �����
    Transform target;
    public DamageCount damageCount;

    public bool isAnimationIdle = false; // ����������, ����� ���� �� ��������, ����� ������������� �������� idle. ��-�� ���������� Strafe
    public bool isAttacking = false; // ���� true, ������ �������� �� �����

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
    public float CheckAngle() // ��������� ���� ����� ������������ ������� ����� � ������
    {
        vectorToPlayer = (player.position - enemy.position);
        vectorToPlayer.y = 0;
        vectorToPlayer.Normalize();

        enemyForward = enemy.forward;
        enemyForward.y = 0;
        enemyForward.Normalize();
        return Vector3.SignedAngle(vectorToPlayer, enemyForward, Vector3.up);
    }
    public float CheckDistance() // ��������� ���������� ����� ������ � �������
    {
        return (transform.position - target.transform.position).magnitude;
    }
    public void ResetAttack() // � idle animation event ������� ����������� �������� �����
    {
        animator.SetBool("AttackLeft", false);
        animator.SetBool("AttackRight", false);
        damageCount.attacking = false;
        isAttacking = false;
    }
    public void RandomAttack() 
    {
        int randomInt = Random.Range(0, 2);
        int chanceOfAttack = Random.Range(0, 2);
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
        isAttacking = true;
    }
    public void CanHitPlayer()
    {
        damageCount.attacking = true;
    }
    public void CannotHitPlayer()
    {
        damageCount.attacking = false;
    }
    public void EndAttackAnimation()
    {
        isAttacking = false;
    }
    private void Update()
    {
        SetTarget(player); // ������ �������� target Transform player
        navMeshAgent.destination = target.position; // ��������� ��������� �������
        currentState.UpdateState(this); // ���������� ����� Updatestate() �������� ���������
    }
}
