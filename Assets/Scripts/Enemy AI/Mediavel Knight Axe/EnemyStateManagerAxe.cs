using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManagerAxe : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] public Transform player; // Transform ������
    [SerializeField] public Transform enemy; // Transform �����
    [SerializeField] public float walkSpeed; // �������� ������ �����
    [SerializeField] public float agroDistance; // ��������� ���� �����
    [SerializeField] public float attackDistance; // ��������� ����� �����
    [SerializeField] public float angleSpeed = 28f; // �������� Strafe �����
    [SerializeField] public Animator animator; // �������� �����
    [SerializeField] public float retreatSpeed = 1f; // ��������� Strafe �����
    public DamageCount damageCount;

    public bool isAttacking = false; // ���� true, ������ �������� �� �����

    public Vector3 vectorToPlayer; // ������ �� ����� � ������
    public Vector3 enemyForward; // ������ ����������� ������� �����
    ZoneTriggerManagerAxe zoneManager; // �������� ���, ������� �������� �� ��, � ����� ���� �������� ��� ������
    Transform target; // ���� �������������

    BaseStateAxe currentState;
    public IdleStateAxe idleState = new();
    public DefenseStateAxe defenceState = new();
    public AttackStateAxe attackState = new();
    public AgroStateAxe agroState = new();
    public RetreatStateAxe retreatState = new();

    public void SwitchState(BaseStateAxe newState) // �����, �������� ������� ���������
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

    public void StartAnimationDown() // ����������� ����� �������� ��� ��������� ���� down ����������. Animation event
    {
        enemy.position = Vector3.MoveTowards(enemy.position, enemy.position - enemy.forward * 2f, 4f * Time.deltaTime);
    }
    public void StartRetreatMove()
    {
        if (!isAttacking)
        {
            enemy.position = Vector3.Lerp(enemy.position, enemy.position - enemy.forward * 2f, retreatSpeed * Time.deltaTime); // ������ ������� ����� �� ������������ ����������
        }
    }
    public void EndAnimationDown() // ����������� ����� �������� ��� ��������� ���� down �������������. Animation event
    {
        zoneManager.defenseSide = "";
    }
    public void StartAttackAnimation()
    {
        zoneManager.strafing = false;
        animator.SetBool("StrafeL", false);
        animator.SetBool("StrafeR", false);
    }
    public void CanHitPlayer()
    {
        damageCount.attacking = true;
    }
    public void CannotHitPlayer()
    {
        damageCount.attacking = false;
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
        SwitchState(idleState); // �������� ����������� ���������
    }
    private void Update()
    {
        if (damageCount.revive)
        {
            zoneManager.defenseSide = "";
            SwitchState(idleState);
            damageCount.revive = false;
        }
        SetTarget(player); // ������ �������� target Transform player
        navMeshAgent.destination = target.position; // ��������� ��������� �������
        currentState.UpdateState(this, zoneManager); // ���������� ����� Updatestate() �������� ���������
    }
}
