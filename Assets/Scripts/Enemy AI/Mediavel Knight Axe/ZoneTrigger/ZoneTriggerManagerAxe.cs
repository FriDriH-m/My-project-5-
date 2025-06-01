using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneTriggerManagerAxe : MonoBehaviour
{
    [SerializeField] Animator animator;
    EnemyStateManagerAxe manager;

    public float defenceTime = 0; // время, в течение которого враг защищается от атаки игрока
    public bool strafing = false;

    public string defenseSide = "";

    List<string> attackZone = new List<string> { "attackMiddle", "attack1", "attack2", "attack360" }; // список комбинаций атак

    private void Start()
    {
        manager = GetComponent<EnemyStateManagerAxe>();

    }
    public void StartIdleAnimation() // Animation event у idle анимации в начале
    {
        for (int i = 0; i  < attackZone.Count; i++)
        {
            animator.SetBool(attackZone[i], false);
        }
        manager.damageCount.attacking = false;
        manager.isAttacking = false;
    }
    public void AttackAnimation()
    {
        if (!manager.isAttacking)
        {            
            int chanceOfAttack = Random.Range(0, 4); // Шанс, что враг осмелится атаковать игрока
            int randAttackInteger = Random.Range(0, 4); // рандомный выбор из доступных атак
            if (chanceOfAttack == 1)
            {
                animator.SetBool(attackZone[randAttackInteger], true);
                manager.isAttacking = true;                
            }
        }
        
    }
    public void StrafeAnimation()
    {
        if (!strafing && defenseSide == "" && !manager.isAttacking)
        {
            int randInt = Random.Range(0, 2); 
            if (randInt == 0 )
            {
                animator.SetBool("StrafeL", true);
                animator.SetBool("StrafeR", false);
                strafing = true;
            }
            else if (randInt == 1)
            {
                animator.SetBool("StrafeR", true);
                animator.SetBool("StrafeL", false);
                strafing = true;
            }            
        }
        if (animator.GetBool("StrafeL"))
        {
            manager.enemy.RotateAround(manager.player.position, Vector3.up, manager.angleSpeed * Time.deltaTime);
        }

        if (animator.GetBool("StrafeR"))
        {
            manager.enemy.RotateAround(manager.player.position, Vector3.up, -1 * manager.angleSpeed * Time.deltaTime);
        }
    }
    public void DefenseAnimation()
    {
        if (!manager.isAttacking)
        {
            if (defenseSide == "top")
            {
                manager.animator.SetBool("top", true);
                defenceTime += Time.deltaTime;
            }
            else { manager.animator.SetBool("top", false); }

            if (defenseSide == "down")
            {
                manager.animator.SetBool("down", true);
                defenceTime += Time.deltaTime;
                manager.StartAnimationDown();
            }
            else { manager.animator.SetBool("down", false); }
        }
    }
}
