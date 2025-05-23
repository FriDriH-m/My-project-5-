using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.Collections.Generic;

public class BossStateManager : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
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
    public bool attackMove;
    public bool canSwitchState;
    public bool isAttacking = false;
    bool _isStrafing = false;
    public int _strafingSide;

    BaseStateBoss currentState;
    public IdleStateB idleState = new();
    public DefenseStateB defenseState = new();
    public AttackStateB attackState = new();
    public AgroStateB agroState = new();

    List<string> attacks = new List<string> { "Attack R", "Attack L", "Attack L inplace" };
    List<string> moveAttacks = new List<string> { "Attack R move1", "Attack L move" };

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
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

    public void FastDistanceAttack(Coroutine _runCoroutine)
    {
        if (CheckDistance() <= attackDistance && _runCoroutine != null && !isAttacking && animator.GetBool("Run"))
        {
            StopCoroutine(_runCoroutine);
            _runCoroutine = null;

            SetSpeed(0);
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.nextPosition = enemy.transform.position;

            isAttacking = true;
            animator.SetBool("Attack R move", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            canSwitchState = false;
        }
    }
    public void GetCloser(bool runAttack)
    {
        if (attackMove)
        {
            float moveSpeed;
            if (runAttack)
            {
                moveSpeed = 20;
            }
            else moveSpeed = 10;
            float step = moveSpeed * Time.deltaTime;
            enemy.position = Vector3.MoveTowards(
                enemy.position,
                player.position,
                step
            );

            if (Vector3.Distance(enemy.position, player.position) <= 2.5f)
            {
                attackMove = false;
                animator.SetBool("Attack R move", false);
                canSwitchState = true;
            }
        }
    }
    public void Strafing()
    {
        if (!_isStrafing && !isAttacking)
        {
            if (_strafingSide == 1)
            {
                animator.SetBool("Walk Left", false);
                animator.SetBool("Walk Right", true);
            }
            else
            {
                animator.SetBool("Walk Right", false);
                animator.SetBool("Walk Left", true);
            }
        }

        if (_isStrafing && !isAttacking)
        {
            if (animator.GetBool("Walk Left"))
            {
                enemy.RotateAround(player.position, Vector3.up, angleSpeed * Time.deltaTime);
            }
            else enemy.RotateAround(player.position, Vector3.up, -1 * angleSpeed * Time.deltaTime);
        }
    }
    public void Attack()
    {
        int chanceOfAttack = Random.Range(0, 5);
        if (!isAttacking && chanceOfAttack == 1)
        {
            if (CheckDistance() > attackDistance - 4)
            {
                int randInt = Random.Range(0, 2);
                animator.SetBool(moveAttacks[randInt], true);
                isAttacking = true;
            }
            else
            {
                int randInt = Random.Range(0, 3);
                animator.SetBool(attacks[randInt], true);
                isAttacking = true;
            }
        }
    }
    public void JumpBack()
    {
        if (attackMove)
        {
            float moveSpeed = 20;
            float step = moveSpeed * Time.deltaTime;
            enemy.position = Vector3.MoveTowards(
                enemy.position,
                player.position,
                -step
            );
            canSwitchState = false;
        }
        if (Vector3.Distance(enemy.position, player.position) > 6f)
        {
            attackMove = false;
            canSwitchState = true;
        }
    }

    public void StartStrafe()
    {
        _isStrafing = true;
    }
    public void EndStrafeAnimation()
    {
        _isStrafing = false;
    }
    public void MoveAttack()
    {
        attackMove = true;
        canSwitchState = false;
    }
    private void EndImpactAnimation()
    {
        animator.SetBool("TorsoImpact", false);
    }
    public void StartIdleAnimation()
    {
        for (int i = 0; i < 3; i++)
        {
            animator.SetBool(attacks[i], false);
        }
        for (int i = 0; i < 2; i++)
        {
            animator.SetBool(moveAttacks[i], false);
        }
        isAttacking = false;
        _isStrafing = false;
    }
}