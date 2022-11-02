using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTeamCover : MonoBehaviour
{
    CoverType cover = CoverType.None;
    public CoverType Cover { get { return cover; } set { cover = value; } }

    Cover[] covers;

    private void Update()
    {
        SetCover();
        SetCoverIcon();
    }

    void SetCover()
    {
        covers = FindObjectsOfType<Cover>();

        foreach (Cover c in covers)
        {
            float distanceToTarget = Mathf.Infinity;
            distanceToTarget = Vector3.Distance(transform.position, c.transform.position);

            if (distanceToTarget <= c.CoverRange)
            {
                cover = c.CoverType;

                return;
            }
        }

        cover = CoverType.None;
    }

    void SetCoverIcon()
    {
        GameObject coverIcon = transform.GetChild(1).gameObject;
        var coverIconRenderer = coverIcon.GetComponent<Renderer>();

        if (cover == CoverType.LightCover)
        {
            coverIcon.SetActive(true);
            coverIconRenderer.material.SetColor("_Color", Color.yellow);
        }
        else if(cover == CoverType.HardCover)
        {
            coverIcon.SetActive(true);
            coverIconRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            coverIcon.SetActive(false);
        }
        
    }
}
