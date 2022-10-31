using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireTeam : IFireTeam
{
    bool isSelected = false;
    public bool IsSelected { get { return isSelected; } }

    int hitPoints = 100;

    bool isDead = false;
    bool isHidden = false;

    FireTeamCover cover;

    private void Start()
    {
        FireTeamSelections.Instance.fireTeamList.Add(gameObject);
        cover = GetComponent<FireTeamCover>();

    }

    private void OnDestroy()
    {
        FireTeamSelections.Instance.fireTeamList.Remove(gameObject);
    }

    public void SelectFireTeam()
    {
        isSelected = true;
    }

    public void DeSelectFireTeam()
    {
        isSelected = false;
    }

    public override void TakeDamage(int damage)
    {
        if(cover.Cover == CoverType.LightCover)
        {
            hitPoints -= Convert.ToInt32(damage * 0.7);
        } else if(cover.Cover == CoverType.HardCover)
        {
            hitPoints -= Convert.ToInt32(damage * 0.3);
        } else
        {
            hitPoints -= damage;
        }

        if (hitPoints <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        if (isDead) return;

        isDead = true;
        Destroy(gameObject, 0.3f);
        GetComponent<Animator>().SetTrigger("die");
        GetComponent<FireTeamAttack>().enabled = false;
        GetComponent<FireTeamMovement>().enabled = false;
        FireTeamSelections.Instance.fireTeamList.Remove(gameObject);
        FireTeamSelections.Instance.fireTeamsSelected.Remove(gameObject);
        transform.GetChild(0).gameObject.SetActive(false);
        // send morale penaly, TOOD: unitfactor type property?
        SendMessageUpwards("FireTeamKilled", 10);
    }
}

