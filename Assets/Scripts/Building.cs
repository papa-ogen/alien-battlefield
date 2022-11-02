using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using static UnityStandardAssets.Utility.TimedObjectActivator;

public class Building : MonoBehaviour
{
    [SerializeField] int size = 3;
    [SerializeField] CoverType cover = CoverType.HardCover;
    [SerializeField] List<FireTeam> occupants;

    Color startcolor;
    Material child;
    TextMeshPro enterExitText;
    TextMeshPro instructionsText;

    private void Awake()
    {
        child = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        startcolor = child.color;
    }

    private void Start()
    {
        enterExitText = GetComponentsInChildren<TextMeshPro>()[0];
        enterExitText.enabled = false;

        instructionsText = GetComponentsInChildren<TextMeshPro>()[1];
        instructionsText.enabled = false;

        AssignOccupants();
    }

    private void OnMouseOver()
    {
        List<FireTeam> selectedFireTeams = FireTeamSelections.Instance.fireTeamsSelected;

        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LeaveBuilding());
        }

        if (occupants.Count == size)
        {
            DisplayText("Building Full", "(left click to empty)");
            child.color = Color.red;
        } else if (selectedFireTeams.Count > 0)
        {
            DisplayText("Enter Building", "(right click to enter)");
            child.color = Color.yellow;

            if (Input.GetMouseButtonDown(1))
            {

                EnterBuilding(selectedFireTeams);
            }
        }
    }

    private void OnMouseExit()
    {
        child.color = startcolor;

        HideText();
    }

    private void HideText()
    {
        enterExitText.enabled = false;
        instructionsText.enabled = false;
    }

    void DisplayText(string enterExitText, string instructionsText)
    {
        if (this.enterExitText == null) return;

        this.enterExitText.text = enterExitText;
        this.enterExitText.enabled = true;

        if (this.instructionsText == null) return;

        this.instructionsText.text = instructionsText;
        this.instructionsText.enabled = true;

        // TODO: Look at camera
        // enterExitText.transform.LookAt(Camera.main.transform);
    }

    void AssignOccupants()
    {
        foreach (FireTeam occupant in occupants)
        {
            occupant.transform.position = transform.position;
            occupant.Cover.Cover = CoverType.HardCover;
        }
    }

    IEnumerator LeaveBuilding()
    {
        Vector3 offset = new Vector3(-5, 0, -10);
        foreach (FireTeam occupant in occupants)
        {
            occupant.transform.position = gameObject.transform.position - offset;
            offset = new Vector3(0, 0, 5) + offset;

            occupants.Remove(occupant);

            yield return new WaitForSeconds(1);
        }
    }

    private void EnterBuilding(List<FireTeam> selectedFireTeams)
    {
        foreach (FireTeam selectedFireTeam in selectedFireTeams)
        {
            float distanceToTarget = Vector3.Distance(transform.position, selectedFireTeam.transform.position);

            if (distanceToTarget <= 10f)
            {
                selectedFireTeam.transform.position = transform.position;
                selectedFireTeam.Cover.Cover = CoverType.HardCover;
                occupants.Add(selectedFireTeam);
            }
        }
    }
}
