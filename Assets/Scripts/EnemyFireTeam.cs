using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using System;

public class EnemyFireTeam : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] CoverType hasCover;
    public CoverType HasCover { get { return hasCover; } }

    bool isHidden = false;
    public bool IsHidden { get { return isHidden; } }

    Cover[] covers;

    private void Update()
    {
        FindCover();
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FindCover()
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
