using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireTeamMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    bool advancing = true;
    public bool Advancing { get { return advancing; } set { advancing = value; } }

    private void Update()
    {
        if(advancing)
        {
            transform.position -= Vector3.forward* Time.deltaTime* movementSpeed;
        } else
        {
            transform.position += Vector3.forward * Time.deltaTime * movementSpeed;
        }
        /*
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
        {
            myAgent.SetDestination(hit.point);
        }
        */
    }
}
