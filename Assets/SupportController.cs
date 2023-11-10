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

    private void Update()
    {
        if (_unit.reachedEndOfPath)
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
        }
    }


    public void SetTarget(Vector3 targetPos)
    {
        _unit.SetDestination(targetPos);
    }
    
    public void SetController(CharacterManager _controller) => mainController = _controller;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
