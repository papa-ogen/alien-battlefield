using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using System;
using static UnityStandardAssets.Utility.TimedObjectActivator;

public class EnemyFireTeam : MonoBehaviour
{
    [SerializeField] int hitPoints = 100;
    public float HitPoints { get { return hitPoints; } }    
    [SerializeField] CoverType cover;
    [SerializeField] float scoutRange = 30f;

    bool isDead = false;
    public bool IsDead { get { return isDead; } }

    bool isHidden = false;
    public bool IsHidden { get { return isHidden; } }

    Cover[] covers;
    FireTeam[] enemies;
    FireTeam targetEnemy;
    public FireTeam TargetEnemy { get { return targetEnemy; } }
    

    private void Update()
    {
        LookForCover();
        LookForEnemies();

        if (targetEnemy == null)
        {
            if(cover == CoverType.None)
            {
                Advance();
            } else
            {
                Attack();
            }
        }
        else
        {
            if(cover == CoverType.None)
            {
                Retreat();
            } else
            {
                Attack();
            }
        }

        // TODO : Passive AI
        /* if no enemy
         * 1. Look For Cover
         * 2 If Cover, IDLE
         * 3. If No cover, move "forward
         * 
         * If Enemy
         * If Cover -> ATTACK
         * If no cover, look for cover
         * If no cover, move away from enemy
         * 
        */
        if(cover != CoverType.None)
        {
            GetComponent<EnemyFireTeamMovement>().enabled = false;
        }

    }

    private void Attack()
    {
        Debug.Log("Attacking");
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<EnemyFireTeamAttack>().enabled = true;
    }

    private void Advance()
    {
        GetComponent<Animator>().SetBool("move", false);
        GetComponent<EnemyFireTeamMovement>().enabled = true;
        GetComponent<EnemyFireTeamAttack>().enabled = false;

        // if cover found, move towards cover
    }

    private void Retreat()
    {
        GetComponent<Animator>().SetBool("move", false);
        GetComponent<EnemyFireTeamMovement>().enabled = true;
        GetComponent<EnemyFireTeamMovement>().Advancing = false;
        GetComponent<EnemyFireTeamAttack>().enabled = false;

        // if cover found, move towards cover
    }

    public void TakeDamage(int damage)
    {
        if (cover == CoverType.LightCover)
        {
            hitPoints -= Convert.ToInt32(damage * 0.7);
        }
        else if (cover == CoverType.HardCover)
        {
            hitPoints -= Convert.ToInt32(damage * 0.3);
        }
        else
        {
            hitPoints -= damage;
        }

        if (hitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        GetComponent<Animator>().SetTrigger("die");
        GetComponent<EnemyFireTeamAttack>().enabled = false;
    }

    void LookForCover()
    {
        covers = FindObjectsOfType<Cover>();

        foreach (Cover c in covers)
        {
            float distanceToTarget = Mathf.Infinity;
            distanceToTarget = Vector3.Distance(transform.position, c.transform.position);

            if (distanceToTarget <= c.CoverRange)
            {
                cover = c.CoverType;

                return;
            }
        }

        cover = CoverType.None;
    }

    void LookForEnemies()
    {
        enemies = FindObjectsOfType<FireTeam>();

        foreach (FireTeam enemy in enemies)
        {
            float distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToTarget <= scoutRange && !enemy.IsDead)
            {
                targetEnemy = enemy;

                return;
            }
        }
    }
}
