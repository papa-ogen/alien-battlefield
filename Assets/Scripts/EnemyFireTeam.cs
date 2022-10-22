using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

[RequireComponent(typeof(FireTeamHealth))]
public class EnemyFireTeam : MonoBehaviour
{
    FireTeamHealth health;

    void Start()
    {
        health = GetComponent<FireTeamHealth>();
    }
}
