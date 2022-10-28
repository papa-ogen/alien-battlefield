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

    bool isHidden = false;
    public bool IsHidden { get { return isHidden; } }

    Cover[] covers;
    FireTeam targetEnemy;
    FireTeam[] enemies;

    private void Update()
    {
        /*
        IsInCover();

        if (targetEnemy == null)
        {
            LookForEnemies();
        }
        else if (targetEnemy != null && canAttack)
        {
            StartCoroutine(Attack());
        }
        */
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
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

    void LookForEnemies()
    {
        enemies = FindObjectsOfType<FireTeam>();

        foreach (FireTeam enemy in enemies)
        {
            if (!enemy.IsHidden)
            {
                float distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToTarget <= attackRange)
                {
                    targetEnemy = enemy;

                    transform.LookAt(targetEnemy.transform);

                    return;
                }
            }
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        SetTracersActive(true);

        // Give damage method - if in cover for example
        targetEnemy.TakeDamage(attackDamage);

        PlayMuzzleFlash();

        yield return new WaitForSeconds(attackSpeed);

        SetTracersActive(false);
        canAttack = true;
    }

    private void PlayMuzzleFlash()
    {
        attackEffect.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    [System.Obsolete]
    private void SetTracersActive(bool isActive)
    {
        attackEffect.enableEmission = isActive;
        // attackEffect.emission.enabled = !isActive;
    }
}
