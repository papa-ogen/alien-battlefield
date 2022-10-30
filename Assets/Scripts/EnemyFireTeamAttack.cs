using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireTeamAttack : MonoBehaviour
{
    [SerializeField] float attackRange = 30f;
    [SerializeField] int attackDamage = 30;
    [SerializeField] float attackSpeed = 3f;
    [SerializeField] ParticleSystem attackEffect;

    [SerializeField] bool canAttack = true;

    FireTeam targetEnemy;

    private void OnEnable()
    {
        targetEnemy = GetComponent<EnemyFireTeam>().TargetEnemy;
    }

    void Update()
    {
        if(targetEnemy && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        GetComponent<Animator>().SetBool("attack", true);
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
