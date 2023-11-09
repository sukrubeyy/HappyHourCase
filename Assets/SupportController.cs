using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportController : MonoBehaviour
{
    public Unit _unit;
    public SupportType type;
    [Range(0, 5)] public float radius;
    public Color gizmosColor;
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawSphere(transform.position,radius);
    }
}
