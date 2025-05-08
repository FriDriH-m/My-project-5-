using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneTriggerManagerAxe : MonoBehaviour
{
    [SerializeField] Animator animator;
    EnemyStateManagerAxe manager;

    public float defenceTime = 0; // время, в течение которого враг защищается от атаки игрока

    public int top = 0; 
    public int down = 0; // переменные = 1, когда оружие игрока заходит в зону защиты врага

    List<string> attackZone = new List<string> { "attackMiddle", "attack1", "attack2", "attack360" }; // список комбинаций атак

    private void Start()
    {
        manager = GetComponent<EnemyStateManagerAxe>();
    }
    public void SetActiveZone(string zone) // в триггер зонах врага активируется, когда меч игрока входит в зону
    {
        switch (zone)
        {
            case "top": top = 1; 
                break;
            case "down": down = 1;
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
            default:
                Debug.LogWarning($"Unknown zone: {zone}");
                break;

        }
    }
    public void ResetAttackAnimation() // Animation event у idle анимации в начале
    {
        animator.SetBool("attack2", false);
        animator.SetBool("attack1", false);
        animator.SetBool("attackMiddle", false);
        animator.SetBool("attack360", false);
    }
    public void AttackAnimation()
    {
        int chanceOfAttack = Random.Range(0, 5); // Шанс, что враг осмелится атаковать игрока
        int randAttackInteger = Random.Range(0, 4); // рандомный выбор из доступных атак
        if (chanceOfAttack == 1) 
        {
            animator.SetBool(attackZone[randAttackInteger], true);
        }
    }
    public void StrafeAnimation()
    {
        if (!manager.isAnimation && !manager.isAnimationDown && !manager.isAttacking)
        {
            animator.SetBool("StrafeR", false);
            animator.SetBool("StrafeL", false);

            int randInt = Random.Range(0, 2); 
            if (randInt == 0 )
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
            else if (animator.GetBool("StrafeR"))
            {
                manager.enemy.RotateAround(manager.player.position, Vector3.up, -1 * manager.angleSpeed * Time.deltaTime);
            }
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
        }
    }
}
