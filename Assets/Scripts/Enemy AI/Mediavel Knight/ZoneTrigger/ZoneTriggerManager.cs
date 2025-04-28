using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneTriggerManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    EnemyStateManager manager;

    public float defenceTime = 0; // время, в течение которого враг защищается от атаки игрока
    int _chanceOfCombination; // шанс комбинации атак
    bool _isCombinationAttack = false; // переменная = true, если враг использует комбинацию атак

    public int top = 0; 
    public int down = 0; // переменные = 1, когда оружие игрока заходит в зону защиты врага
    public int left = 0;
    public int right = 0; 

    private AttackMiddleZone attackMiddleZone; // ссылка на зону атаки 
    private AttackLeftZone attackLeftZone; // ссылка на зону атаки 
    private AttackRightZone attackRightZone; // ссылка на зону атаки 

    List<string> activeAttackZone = new List<string>(); // список доступных зон атаки
    List<string> combinationAttack = new List<string> { "attackMiddle", "attackRight", "attackLeft" }; // список комбинаций атак
    bool leftMove = false; // переменная, которая отвечает за направление движения врага при Strafe

    private void Start()
    {
        manager = GetComponent<EnemyStateManager>();
        attackMiddleZone = FindFirstObjectByType<AttackMiddleZone>();
        attackLeftZone = FindFirstObjectByType<AttackLeftZone>();
        attackRightZone = FindFirstObjectByType<AttackRightZone>();
    }
    public void SetActiveZone(string zone) // в триггер зонах врага активируется, когда меч игрока входит в зону
    {
        switch (zone)
        {
            case "top": top = 1; 
                break;
            case "down": down = 1; 
                break;
            case "left": left = 1; 
                break;
            case "right": right = 1; 
                break;
            default: Debug.LogWarning($"Unknown zone: {zone}"); break;
        }
    }
    public void ResetZone(string zone)
    {
        switch (zone)
        {
            case "top":
                top = 0; 
                break;
            case "down":
                down = 0; 
                break;
            case "left":
                left = 0; 
                break;
            case "right":
                right = 0; 
                break;
            default:
                Debug.LogWarning($"Unknown zone: {zone}");
                break;

        }
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
    public void ResetAttackAnimation() // Animation event у idle анимации в начале
    {
        animator.SetBool("attackRight", false);
        animator.SetBool("attackLeft", false);
        animator.SetBool("attackMiddle", false);
        if (_isCombinationAttack)
        {
            animator.SetBool(combinationAttack[Random.Range(0, 3)], true);
            _isCombinationAttack = false;
        }
    }
    public void AttackAnimation()
    {
        int chanceOfAttack = Random.Range(0, 15); // Шанс, что враг осмелится атаковать игрока
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
                leftMove = true;
                animator.SetBool("StrafeL", true);
                animator.SetBool("StrafeR", false);
                manager.isAnimation = true;
            }
            else if (randInt == 2)
            {
                leftMove = false;
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
            if (leftMove)
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
            if (top == 1)
            {
                manager.animator.SetBool("top", true);
                defenceTime += Time.deltaTime;
            }
            else { manager.animator.SetBool("top", false); }

            if (down == 1 && top != 1)
            {
                manager.animator.SetBool("down", true);
                defenceTime += Time.deltaTime;
                manager.StartAnimationDown();
            }
            else { manager.animator.SetBool("down", false); }

            if (left == 1)
            {
                manager.animator.SetBool("left", true);
                defenceTime += Time.deltaTime;
            }
            else { manager.animator.SetBool("left", false); }

            if (right == 1)
            {
                manager.animator.SetBool("right", true);
                defenceTime += Time.deltaTime;
            }
            else { manager.animator.SetBool("right", false); }
        }
    }
}
