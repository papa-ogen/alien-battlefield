using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(ClickToMove))]
public class FireTeam : MonoBehaviour
{
    [SerializeField] bool isSelected = false;
    public bool IsSelected { get { return isSelected; }}

    [Tooltip("Selection indicator")]
    [SerializeField] GameObject selectedBase;
    [SerializeField] float hitPoints = 100f;
    public float HitPoints { get { return hitPoints; } }

    CoverType hasCover;
    GameObject hasCoverIcon;
    ClickToMove moveFireTeam;
    Cover[] covers;


    bool isDead = false;
    public bool IsDead { get { return isDead; } }

    private void Awake()
    {
        moveFireTeam = GetComponent<ClickToMove>();
        hasCoverIcon = GameObject.Find("hasCoverIcon");
    }

    private void Start()
    {
        selectedBase.SetActive(false);
        moveFireTeam.enabled = false;

        hasCoverIcon.SetActive(false);
    }

    private void Update()
    {
        FindCover();
    }

    public void SelectFireTeam()
    {
        isSelected = true;
        selectedBase.SetActive(true);
        moveFireTeam.enabled = true;
        hasCoverIcon.SetActive(true);
    }

    public void DeSelectFireTeam()
    {
        isSelected = false;
        selectedBase.SetActive(false);
        moveFireTeam.enabled = false;
        hasCoverIcon.SetActive(false);
    }

    void FindCover()
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
                    hasCoverIcon.GetComponent<MeshRenderer>().material.color = Color.red;
                } else
                {
                    hasCoverIcon.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }

                return;
            }
        }

        hasCover = CoverType.None;
        hasCoverIcon.GetComponent<MeshRenderer>().material.color = Color.white;
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
        if (isDead) return;

        isDead = true;
    }
} 
