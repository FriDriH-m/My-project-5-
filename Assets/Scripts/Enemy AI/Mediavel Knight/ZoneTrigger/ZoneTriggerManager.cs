using System.Collections.Generic;
using UnityEngine;

public class ZoneTriggerManager : MonoBehaviour
{
    Animator animator;
    EnemyStateManager manager;

    public float defenceTime = 0; // �����, � ������� �������� ���� ���������� �� ����� ������
    int _chanceOfCombination; // ���� ���������� ����
    bool _isCombinationAttack = false; // ���������� = true, ���� ���� ���������� ���������� ����

    public string defenseSide = ""; // �������, � ������� ���� ���������� �� ����� ������

    private AttackMiddleZone attackMiddleZone; // ������ �� ���� ����� 
    private AttackLeftZone attackLeftZone; // ������ �� ���� ����� 
    private AttackRightZone attackRightZone; // ������ �� ���� ����� 

    List<string> activeAttackZone = new List<string>(); // ������ ��������� ��� �����
    List<string> combinationAttack = new List<string> { "attackMiddle", "attackRight", "attackLeft" }; // ������ ���������� ����

    private void Start()
    {
        animator = GetComponent<Animator>(); 
        manager = GetComponent<EnemyStateManager>();
        attackMiddleZone = FindFirstObjectByType<AttackMiddleZone>();
        attackLeftZone = FindFirstObjectByType<AttackLeftZone>();
        attackRightZone = FindFirstObjectByType<AttackRightZone>();
    }
    public bool HasWeapon(Transform parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
    public void SetAttackZone() // ������������ ���� ����� ����� � ���� �� ��� ������ ������ ��� ������
    {
        activeAttackZone.Clear();
        if (attackLeftZone.haveWeapon) // ���� ����� � ���� ����� �����
        {
            activeAttackZone.AddRange(new List<string> { "attackMiddle", "attackRight" });
        }
        else if (attackRightZone.haveWeapon) // ���� ����� � ���� ����� ������
        {
            activeAttackZone.AddRange(new List<string> { "attackMiddle", "attackLeft" });
        }
        else if (attackMiddleZone.haveWeapon) // ���� ����� � ���� ����� ����������
        {
            activeAttackZone.AddRange(new List<string> { "attackLeft", "attackRight" });
        }
        else
        {
            activeAttackZone.AddRange(new List<string> { "attackLeft", "attackRight", "attackMiddle"});
        }
    }
    public void StartIdleAnimation() // Animation event � idle �������� � ������
    {
        animator.SetBool("attackRight", false);
        animator.SetBool("attackLeft", false);
        animator.SetBool("attackMiddle", false);
        manager.isAttacking = false;
        if (_isCombinationAttack)
        {
            animator.SetBool(combinationAttack[Random.Range(0, 3)], true);
            _isCombinationAttack = false;
        }
    }
    public void StartRetreat()
    {
        defenseSide = "down"; 
        defenceTime = 0;
    }
    public void AttackAnimation()
    {
        int chanceOfAttack = Random.Range(0, 4); // ����, ��� ���� ��������� ��������� ������
        _chanceOfCombination = Random.Range(0, 4); // ����, ��� ���� ������� �����������
        int randAttackInteger = Random.Range(0, 2); // ��������� ����� �� ��������� ����

        SetAttackZone(); // ������ ���� �����

        if (!HasWeapon(manager.player, "Weapon") && !(manager.CheckDistance() > manager.attackDistance)) // ���� � ������ ��� ������, �� ���� ���������� � retreat state � agro, �� ���� ����� ��������� ��� ���������
        {
            chanceOfAttack = 1;
            randAttackInteger = Random.Range(0, 3);
            activeAttackZone = new List<string> { "attackMiddle", "attackRight", "attackLeft" };
            animator.SetBool(activeAttackZone[randAttackInteger], true);
        }
        if (chanceOfAttack == 1) // ���� ���� ������
        {
            animator.SetBool(activeAttackZone[randAttackInteger], true);
            if (_chanceOfCombination == 0)
            {
                _isCombinationAttack = true;
            }
        }
    }
    public void StrafeAnimation()
    { 
        if (!manager.isAnimation && !manager.isAnimationDown && !manager.isAttacking)
        {
            animator.SetBool("StrafeR", false);
            animator.SetBool("StrafeL", false);

            int randInt = Random.Range(0, 2);
            if (randInt == 0)
            {
                animator.SetBool("StrafeL", true);
                animator.SetBool("StrafeR", false);
                manager.isAnimation = true;
            }
            else if (randInt == 1)
            {
                animator.SetBool("StrafeR", true);
                animator.SetBool("StrafeL", false);
                manager.isAnimation = true;
            }
        }
        if ((animator.GetBool("StrafeL") || animator.GetBool("StrafeR")) && !manager.isAttacking)
        {
            if (animator.GetBool("StrafeL"))
            {
                manager.enemy.RotateAround(manager.player.position, Vector3.up, manager.angleSpeed * Time.deltaTime);
            }
            else manager.enemy.RotateAround(manager.player.position, Vector3.up, -1 * manager.angleSpeed * Time.deltaTime);
        }
    }
    public void DefenseAnimation()
    {
        if (!manager.isAttacking)
        {            
            switch (defenseSide)
            {
                case "":
                    animator.SetBool("down", false);
                    animator.SetBool("right", false);
                    animator.SetBool("left", false);
                    animator.SetBool("top", false);
                    defenceTime = 0;
                    break;
                case "top":
                    animator.SetBool("down", false);
                    animator.SetBool("right", false);
                    animator.SetBool("left", false);
                    animator.SetBool("top", true);
                    break;
                case "left":
                    animator.SetBool("down", false);
                    animator.SetBool("right", false);
                    animator.SetBool("top", false);
                    animator.SetBool("left", true);
                    break;
                case "right":
                    animator.SetBool("down", false);
                    animator.SetBool("left", false);
                    animator.SetBool("top", false);
                    animator.SetBool("right", true);
                    break;
                case "down":
                    animator.SetBool("right", false);
                    animator.SetBool("left", false);
                    animator.SetBool("top", false);
                    animator.SetBool("down", true);
                    break;
            }
        }
    }
}
