using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyKillTeam : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI killTeamText;
    [SerializeField] TextMeshProUGUI moraleText;

    EnemyFireTeam[] enemyFireTeams;
    bool isDebug;
    int morale = 100;

    void Start()
    {
        enemyFireTeams = GetComponentsInChildren<EnemyFireTeam>();
        killTeamText.enabled = false;
        moraleText.enabled = false;
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            isDebug = !isDebug;
        }

        if (isDebug)
        {
            killTeamText.enabled = true;
            moraleText.enabled = true;
            DisplayKillTeam();
            DisplayMorale();
        } else
        {
            killTeamText.enabled = false;
            moraleText.enabled = false;
        }

    }

        private void DisplayKillTeam()
    {
        killTeamText.text = "";

        foreach (EnemyFireTeam enemyFireTeam in enemyFireTeams)
        {
            killTeamText.text += enemyFireTeam.name + " (" + enemyFireTeam.HitPoints + ")\n";
        }

    }

    private void DisplayMorale()
    {
        moraleText.text = "Morale: " + morale.ToString();
    }
}
