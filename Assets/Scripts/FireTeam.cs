using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireTeam : MonoBehaviour
{
    bool isSelected = false;
    public bool IsSelected { get { return isSelected; } }

    [SerializeField] float hitPoints = 100f;
    public float HitPoints { get { return hitPoints; } }

    [SerializeField] float attackRange = 30f;
    [SerializeField] float attackDamage = 30f;
    [SerializeField] float attackSpeed = 3f;
    //[SerializeField] ParticleSystem attackEffect;
    [SerializeField] bool canAttack = true;
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    CoverType hasCover;
    Cover[] covers;
    EnemyFireTeam[] enemies;
    EnemyFireTeam targetEnemy;
    public EnemyFireTeam TargetEnemy { get { return targetEnemy; } }

    bool isDead = false;
    public bool IsDead { get { return isDead; } }
    bool isHidden = false;
    public bool IsHidden { get { return isHidden; } }

    private void Start()
    {
        FireTeamSelections.Instance.fireTeamList.Add(gameObject);
    }

    private void OnDestroy()
    {
        FireTeamSelections.Instance.fireTeamList.Remove(gameObject);
    }

    private void Update()
    {
        /*
        IsInCover();

        if (targetEnemy == null)
        {
            LookForEnemies();
        } else if (targetEnemy != null && canAttack) {
           StartCoroutine(Attack());
        }
        */
    }

    public void SelectFireTeam()
    {
        isSelected = true;
    }

    public void DeSelectFireTeam()
    {
        isSelected = false;
    }

    void IsInCover()
    {
        covers = FindObjectsOfType<Cover>();

        foreach(Cover cover in covers)
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

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (hitPoints <= 0)
        {
            // send morale penaly, TOOD: unitfactor type property?
            SendMessageUpwards("FireTeamKilled", 10);
            Destroy(gameObject);
        }
    }

    void LookForEnemies()
    {
        enemies = FindObjectsOfType<EnemyFireTeam>();

        foreach (EnemyFireTeam enemy in enemies)
        {
            if(!enemy.IsHidden)
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
      //  attackEffect.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    [System.Obsolete]
    private void SetTracersActive(bool isActive)
    {
       // attackEffect.enableEmission = isActive;
        // attackEffect.emission.enabled = !isActive;
    }
}

