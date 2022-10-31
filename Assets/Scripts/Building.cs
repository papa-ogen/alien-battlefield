using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] int size = 3;
    [SerializeField] CoverType cover = CoverType.HardCover;
    [SerializeField] List<GameObject> occupants;

    Color startcolor;
    Material child;

    private void Awake()
    {
        child = transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        startcolor = child.color;
    }

    private void Start()
    {
        foreach(GameObject occupant in occupants)
        {
            occupant.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        List<GameObject> selectedFireTeams = FireTeamSelections.Instance.fireTeamsSelected;

        // TODO: add fire teams to building, if user gives fire teams new orders. take care of that...

        if(selectedFireTeams.Count > 0)
        {
            child.color = Color.yellow;
        }
    }

    private void OnMouseExit()
    {
        child.color = startcolor;
    }

}
