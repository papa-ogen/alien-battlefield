using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTeamHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;

    bool isDead = false;
    public bool IsDead { get { return isDead; } }

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
        if (isDead) return;

        isDead = true;
    }
}
