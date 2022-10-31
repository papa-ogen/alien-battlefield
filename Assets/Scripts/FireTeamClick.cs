using System.Collections.Generic;
using UnityEngine;

public class FireTeamClick : MonoBehaviour
{
    Camera myCam;

    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;

    void Start()
    {
        myCam = Camera.main;        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    FireTeamSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    FireTeamSelections.Instance.ClickSelect(hit.collider.gameObject);
                }
            }
            else
            {
                if(!Input.GetKey(KeyCode.LeftShift))
                {
                    FireTeamSelections.Instance.DeSelectAll();
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(true);
            }
        }


        IsFireTeamMoving();
    }

    void IsFireTeamMoving()
    {
        List<GameObject> fireTeams = FireTeamSelections.Instance.fireTeamList;

        foreach (GameObject fireTeam in fireTeams)
        {
            if(fireTeam.activeSelf)
            {
                FireTeamMovement ftm = fireTeam.GetComponent<FireTeamMovement>();
                if (ftm.enabled) return;
            }
        }

        groundMarker.SetActive(false);
    }
}
