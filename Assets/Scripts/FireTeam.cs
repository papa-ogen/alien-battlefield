using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(ClickToMove))]
public class FireTeam : MonoBehaviour
{
    bool isSelected = false;
    public bool IsSelected { get { return isSelected; } }

    [Tooltip("Selection indicator")]
    [SerializeField] GameObject selectedBase;
    [SerializeField] float hitPoints = 100f;
    public float HitPoints { get { return hitPoints; } }

    [SerializeField] float attackRange = 30f;
    [SerializeField] float attackDamage = 30f;
    [SerializeField] float attackSpeed = 3f;
    [SerializeField] ParticleSystem attackEffect;
    [SerializeField] bool canAttack = true;
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    CoverType hasCover;
    CoverIcon coverIcon;
    ClickToMove moveFireTeam;
    Cover[] covers;
    EnemyFireTeam[] enemies;
    EnemyFireTeam targetEnemy;
    public EnemyFireTeam TargetEnemy { get { return targetEnemy; } }

    bool isDead = false;
    public bool IsDead { get { return isDead; } }
    bool isHidden = false;
    public bool IsHidden { get { return isHidden; } }

    private void Awake()
    {
        moveFireTeam = GetComponent<ClickToMove>();
        coverIcon = GetComponentInChildren<CoverIcon>();
    }

    private void Start()
    {
        selectedBase.SetActive(false);
        moveFireTeam.enabled = false;

        coverIcon.enabled = false;
    }

    private void Update()
    {
        IsInCover();

        if (targetEnemy == null)
        {
            LookForEnemies();
        } else if (targetEnemy != null && canAttack) {
           StartCoroutine(Attack());
        }
    }

    public void SelectFireTeam()
    {
        isSelected = true;
        selectedBase.SetActive(true);
        moveFireTeam.enabled = true;
        coverIcon.enabled = true;
    }

    public void DeSelectFireTeam()
    {
        isSelected = false;
        selectedBase.SetActive(false);
        moveFireTeam.enabled = false;
        coverIcon.enabled = false;
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

                if(cover.CoverType == CoverType.HardCover)
                {
                    coverIcon.GetComponent<MeshRenderer>().material.color = Color.red;
                } else
                {
                    coverIcon.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }

                return;
            }
        }

        hasCover = CoverType.None;
        coverIcon.GetComponent<MeshRenderer>().material.color = Color.white;
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

