using System.Collections.Generic;
using UnityEngine;

public class ZoneTriggerManager : MonoBehaviour
{
    Animator animator;
    EnemyStateManager manager;

    public float defenceTime = 0; // время, в течение которого враг защищается от атаки игрока
    int _chanceOfCombination; // шанс комбинации атак
    bool _isCombinationAttack = false; // переменная = true, если враг использует комбинацию атак

    public string defenseSide = ""; // сторона, с которой враг защищается от атаки игрока

    private AttackMiddleZone attackMiddleZone; // ссылка на зону атаки 
    private AttackLeftZone attackLeftZone; // ссылка на зону атаки 
    private AttackRightZone attackRightZone; // ссылка на зону атаки 

    List<string> activeAttackZone = new List<string>(); // список доступных зон атаки
    List<string> combinationAttack = new List<string> { "attackMiddle", "attackRight", "attackLeft" }; // список комбинаций атак

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
    public void SetAttackZone() // определяются зоны атаки когда в одну из зон игрока входит его оружие
    {
        activeAttackZone.Clear();
        if (attackLeftZone.haveWeapon) // если игрок в зоне атаки слева
        {
            activeAttackZone.AddRange(new List<string> { "attackMiddle", "attackRight" });
        }
        else if (attackRightZone.haveWeapon) // если игрок в зоне атаки справа
        {
            activeAttackZone.AddRange(new List<string> { "attackMiddle", "attackLeft" });
        }
        else if (attackMiddleZone.haveWeapon) // если игрок в зоне атаки посередине
        {
            activeAttackZone.AddRange(new List<string> { "attackLeft", "attackRight" });
        }
        else
        {
            activeAttackZone.AddRange(new List<string> { "attackLeft", "attackRight", "attackMiddle"});
        }
    }
    public void StartIdleAnimation() // Animation event у idle анимации в начале
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
        int chanceOfAttack = Random.Range(0, 8); // Шанс, что враг осмелится атаковать игрока
        _chanceOfCombination = Random.Range(0, 4); // шанс, что враг атакует комбинацией
        int randAttackInteger = Random.Range(0, 2); // рандомный выбор из доступных атак

        SetAttackZone(); // задает зону атаки

        if (!HasWeapon(manager.player, "Weapon") && !(manager.CheckDistance() < manager.attackDistance - 1.2f) && !(manager.CheckDistance() > manager.attackDistance)) // Если у игрока нет оружия, не надо переходить в retreat state и agro, то враг будет атаковать без остановки
        {
            chanceOfAttack = 1;
            randAttackInteger = Random.Range(0, 3);
            activeAttackZone = new List<string> { "attackMiddle", "attackRight", "attackLeft" };
            animator.SetBool(activeAttackZone[randAttackInteger], true);
        }
        if (chanceOfAttack == 1) // Если есть оружие
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

            int randInt = Random.Range(0, 3);
            if (randInt == 1)
            {
                animator.SetBool("StrafeL", true);
                animator.SetBool("StrafeR", false);
                manager.isAnimation = true;
            }
            else if (randInt == 2)
            {
                animator.SetBool("StrafeR", true);
                animator.SetBool("StrafeL", false);
                manager.isAnimation = true;
            }
            else
            {
                manager.isAnimation = true;
                manager.isAnimationIdle = true;
            }
        }
        if ((animator.GetBool("StrafeL") || animator.GetBool("StrafeR")) && !manager.isAnimationIdle && !manager.isAttacking)
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
