using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField] float coverRange = 10f;
    [SerializeField] CoverType coverType = CoverType.LightCover;
    public CoverType CoverType { get { return coverType; } }

    public float CoverRange { get { return coverRange;} }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, coverRange);
    }
}
