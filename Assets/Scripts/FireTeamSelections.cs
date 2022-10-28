using System.Collections.Generic;
using UnityEngine;

public class FireTeamSelections : MonoBehaviour
{
    public List<GameObject> fireTeamList = new List<GameObject>();
    public List<GameObject> fireTeamsSelected = new List<GameObject>();

    private static FireTeamSelections _instance;
    public static FireTeamSelections Instance { get { return _instance;  } }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ClickSelect(GameObject fireTeamToAdd)
    {
        DeSelectAll();
        fireTeamsSelected.Add(fireTeamToAdd);
        fireTeamToAdd.transform.GetChild(0).gameObject.SetActive(true);
        fireTeamToAdd.GetComponent<FireTeamMovement>().enabled = true;
    }

    public void ShiftClickSelect(GameObject fireTeamToAdd)
    {
        if(!fireTeamsSelected.Contains(fireTeamToAdd))
        {
            fireTeamsSelected.Add(fireTeamToAdd);
            fireTeamToAdd.transform.GetChild(0).gameObject.SetActive(true);
            fireTeamToAdd.GetComponent<FireTeamMovement>().enabled = true;
        }
        else
        {
            fireTeamsSelected.Remove(fireTeamToAdd);
            fireTeamToAdd.transform.GetChild(0).gameObject.SetActive(false);
            fireTeamToAdd.GetComponent<FireTeamMovement>().enabled = false;
        }
    }

    public void DragSelect(GameObject fireTeamToAdd)
    {
        if(!fireTeamsSelected.Contains(fireTeamToAdd))
        {
            fireTeamsSelected.Add(fireTeamToAdd);
            fireTeamToAdd.transform.GetChild(0).gameObject.SetActive(true);
            fireTeamToAdd.GetComponent<FireTeamMovement>().enabled = true;
        }
    }

    public void DeSelectAll()
    {
        foreach (GameObject fireTeam in fireTeamsSelected)
        {
            fireTeam.transform.GetChild(0).gameObject.SetActive(false);
            fireTeam.GetComponent<FireTeamMovement>().enabled = false;
        }

        fireTeamsSelected.Clear();
    }

    public void DeSelect(GameObject fireTeamToDeselect)
    {
        fireTeamToDeselect.transform.GetChild(0).gameObject.SetActive(false);
        fireTeamToDeselect.GetComponent<FireTeamMovement>().enabled = false;
    }
}
