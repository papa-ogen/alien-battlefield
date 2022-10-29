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

    CoverType hasCover;
    Cover[] covers;

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
       // IsInCover();
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

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        GetComponent<Animator>().SetTrigger("die");
        GetComponent<FireTeamAttack>().enabled = false;

        Destroy(gameObject, 2);

        // send morale penaly, TOOD: unitfactor type property?
        // SendMessageUpwards("FireTeamKilled", 10);
    }
}

