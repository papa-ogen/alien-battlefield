using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityStandardAssets.Utility.TimedObjectActivator;

public class KillTeam : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI killTeamText;
    FireTeam[] fireteams;

    void Start()
    {
        fireteams = GetComponentsInChildren<FireTeam>();
        

        Debug.Log("Fire Teams " + fireteams.Length);    
    }

    void Update()
    {
        DisplayKillTeam();
    }

    private void DisplayKillTeam()
    {
        killTeamText.text = "";

        foreach (FireTeam fireTeam in fireteams)
        {
            killTeamText.text += fireTeam.name + " (" + fireTeam.HitPoints + ")\n";
        }

    }
}
