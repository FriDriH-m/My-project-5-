using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public NavMeshAgent navMeshAgent; 
    public Transform player; // Transform ������
    public Transform enemy; // Transform �����
    public float walkSpeed; // �������� ������ �����
    public float agroDistance; // ��������� ���� �����
    public float attackDistance; // ��������� ����� �����
    public float angleSpeed = 28f; // �������� Strafe �����
    public Animator animator; // �������� �����
    public float retreatSpeed = 1f; // ��������� Strafe �����
    public DamageCount damageCount;

    public bool isAnimation = false; // ���������� ��� ���������� Strafe. ����� ��� false, �������� ���������� ��������� ������� Strafe
    public bool isAnimationDown = false; // ����������, ����� ���� ����� ��������� ����� ���� down �� ��������
    public bool isAttacking = false; // ���� true, ������ �������� �� �����

    public Vector3 vectorToPlayer; // ������ �� ����� � ������
    public Vector3 enemyForward; // ������ ����������� ������� �����
    ZoneTriggerManager zoneManager; // �������� ���, ������� �������� �� ��, � ����� ���� �������� ��� ������
    Transform target; // ���� �������������

    BaseState currentState;
    public IdleState idleState = new();
    public DefenceState defenceState = new();
    public AttackState attackState = new();
    public AgroState agroState = new();
    public RetreatState retreatState = new();

    public void SwitchState(BaseState newState) // �����, �������� ������� ���������
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

    public void EndAnimationStrafe() //������ ���� ���� �������� ����������� (Strafe ��������), ������������ Animation event
    {
        isAnimation = false;
    }

    public void StartAnimationDown() // ����������� ����� �������� ��� ��������� ���� down ����������. Animation event
    {
        isAnimationDown = true; 
    }
    public void EndAnimationDown() // ����������� ����� �������� ��� ��������� ���� down �������������. Animation event
    {
        zoneManager.defenseSide = "";
        isAnimationDown = false;
    }

    public void StartRetreatMove()
    {
        if (!isAttacking)
        {
            enemy.position = Vector3.Lerp(enemy.position, enemy.position - enemy.forward * 2f, retreatSpeed * Time.deltaTime); // ������ ������� ����� �� ������������ ����������
        }
    }

    public void StartAttackAnimation()
    {
        animator.SetBool("StrafeR", false);
        animator.SetBool("StrafeL", false);
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
    public void EndImpactAnimation()
    {
        animator.SetBool("HeadImpact", false);
        animator.SetBool("TorsoImpact", false);
    }
    private void Start()
    {
        damageCount = GetComponent<DamageCount>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        zoneManager = GetComponent<ZoneTriggerManager>(); 
        SwitchState(idleState); // �������� ����������� ���������
    }
    private void Update()
    {
        SetTarget(player); // ������ �������� target Transform player
        navMeshAgent.destination = target.position; // ��������� ��������� �������
        currentState.UpdateState(this, zoneManager); // ���������� ����� Updatestate() �������� ���������
    }
}
