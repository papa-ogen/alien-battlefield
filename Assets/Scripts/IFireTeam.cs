using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IFireTeam : MonoBehaviour
{
    [SerializeField] int hitPoints;
    public int HitPoints { get { return hitPoints; } }

    bool isDead;
    public bool IsDead { get { return isDead; } }

    bool isHidden;
    public bool IsHidden { get { return isHidden; } }

    public abstract void TakeDamage(int damage);

    protected abstract void Die();
}
