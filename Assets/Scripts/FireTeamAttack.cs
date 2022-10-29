using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTeamAttack : MonoBehaviour
{
    [SerializeField] float attackRange = 30f;
    [SerializeField] float attackDamage = 30f;
    [SerializeField] float attackSpeed = 3f;
    [SerializeField] ParticleSystem attackEffect;

    [SerializeField] bool canAttack = true;
    // public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    EnemyFireTeam[] enemies;
    EnemyFireTeam targetEnemy;

    void Update()
    {
        if (targetEnemy == null)
        {
            LookForEnemies();
        }
        else if (targetEnemy.IsDead)
        {
            targetEnemy = null;
        }
        else if (targetEnemy != null && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    void LookForEnemies()
    {
        enemies = FindObjectsOfType<EnemyFireTeam>();

        foreach (EnemyFireTeam enemy in enemies)
        {
            float distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToTarget <= attackRange && !enemy.IsDead)
            {
                targetEnemy = enemy;

                transform.LookAt(targetEnemy.transform);

                return;
            }
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        transform.LookAt(targetEnemy.transform);
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

    [System.Obsolete]
    private void SetTracersActive(bool isActive)
    {
        attackEffect.enableEmission = isActive;
        //attackEffect.emission.enabled = !isActive;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
