using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using System;
using static UnityStandardAssets.Utility.TimedObjectActivator;

public class EnemyFireTeam : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    public float HitPoints { get { return hitPoints; } }    
    [SerializeField] CoverType hasCover;
    public CoverType HasCover { get { return hasCover; } }

    [SerializeField] float attackRange = 30f;
    [SerializeField] float attackDamage = 30f;
    [SerializeField] float attackSpeed = 3f;
    [SerializeField] ParticleSystem attackEffect;
    [SerializeField] bool canAttack = true;

    bool isDead = false;
    public bool IsDead { get { return isDead; } }

    bool isHidden = false;
    public bool IsHidden { get { return isHidden; } }

    Cover[] covers;
    FireTeam targetEnemy;
    FireTeam[] enemies;

    private void Update()
    {
        /*
        IsInCover();
        */
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;

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

    void IsInCover()
    {
        covers = FindObjectsOfType<Cover>();

        foreach (Cover cover in covers)
        {
            float distanceToTarget = Mathf.Infinity;
            distanceToTarget = Vector3.Distance(transform.position, cover.transform.position);

            if (distanceToTarget <= cover.CoverRange)
            {
                hasCover = cover.CoverType;

                return;
            }
        }

        hasCover = CoverType.None;
    }
}
