using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectFireTeam : MonoBehaviour
{
    FireTeam fireTeam;
    FireTeam[] fireTeams;

    private void Awake()
    {
        fireTeams = GetComponentsInChildren<FireTeam>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDeSelectAllFireTeams();

            OnSelectFireTeam();
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    OnDeSelectAllFireTeams();
        //}
    }

    void OnSelectFireTeam()
    {
        Debug.Log(Camera.main);

        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            if (raycastHit.transform != null)
            {
                CurrentClickedGameObject(raycastHit.transform.gameObject);
            }
        }
    }

    void CurrentClickedGameObject(GameObject gameObject)
    {
        if (gameObject.tag == "Fire Team")
        {
            fireTeam = gameObject.GetComponent<FireTeam>();
            

            if(!fireTeam.IsSelected)
            {
                fireTeam.SelectFireTeam();
            }

            
        }
    }

    void OnDeSelectAllFireTeams()
    {
        foreach(FireTeam fireTeam in fireTeams)
        {
            fireTeam.DeSelectFireTeam();
        }
    }
}
