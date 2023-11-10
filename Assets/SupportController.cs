using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupportController : MonoBehaviour
{
    public Unit _unit;
    public SupportType type;
    [Range(0, 5)] public float radius;
    public Color gizmosColor;
    private CharacterManager mainController;
    private Wood targetWood;
    private void Update()
    {
        /*if (_unit.reachedEndOfPath)
        {
            var colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (var collider in colliders)
            {
                var wood = collider?.GetComponent<IPickable>();
                if (wood != null)
                {
                    Debug.Log($"Bu Woodu Alan {type}");
                    wood.Pick(mainController);
                }
            }
            _unit.reachedEndOfPath = false;
        }*/

        if (targetWood is not null && Vector3.Distance(targetWood.transform.position, transform.position) <= radius)
        {
            targetWood.GetComponent<IPickable>().Pick(mainController);
            targetWood = null;
        }
    }


    public void SetTarget(Vector3 targetPos)
    {
        _unit.SetDestination(targetPos);
    }
    public void SetTarget(Vector3 targetPos,Wood wood)
    {
        _unit.SetDestination(targetPos);
        targetWood = wood;
    }
    
    public void SetController(CharacterManager _controller) => mainController = _controller;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
