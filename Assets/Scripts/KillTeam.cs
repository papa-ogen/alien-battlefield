using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityStandardAssets.Utility.TimedObjectActivator;

public class KillTeam : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI killTeamText;
    [SerializeField] TextMeshProUGUI killTeamMoraleText;
    FireTeam[] fireTeams;
    int morale = 100;

    void Start()
    {
        fireTeams = GetComponentsInChildren<FireTeam>();

        DisplayKillTeam();
        DisplayMorale();
    }

    private void Update()
    {
        DisplayKillTeam();
        DisplayMorale();
    }

    private void DisplayKillTeam()
    {
        killTeamText.text = "";

        foreach (FireTeam fireTeam in fireTeams)
        {
            killTeamText.text += fireTeam.name + " (" + fireTeam.HitPoints + ")\n";
        }

    }

    public void DisplayMorale()
    {
        killTeamMoraleText.text = "Kill Team (Morale: " + morale + ")";
    }

    void FireTeamKilled(int moralePenalty)
    {
        morale -= moralePenalty;
    }
}
