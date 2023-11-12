using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SupportController : MonoBehaviour
{
    public Unit _unit;
    public SupportType type;
    [Range(0, 5)] public float radius;
    public Color gizmosColor;
    private CharacterManager mainController;
    private Wood targetWood;
    [SerializeField] private PhotonView PV;
    public Color GetColor { get; private set; }

    private void Start()
    {
        GetColor = GetComponent<Renderer>().material.color;
    }

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



    public void SetTarget(Vector3 targetPos, Wood wood)
    {
        _unit.SetDestination(targetPos);
        targetWood = wood;
    }

    public void SetController(CharacterManager _controller)
    {
        mainController = _controller;
        SetColor();
    } 
    
    public void SetColor()
    {
        var newColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        PV.RPC("SetRandomizeColor", RpcTarget.AllBuffered,newColor.r,newColor.g,newColor.b,newColor.a);
    }

    [PunRPC]
    private void SetRandomizeColor(float r,float g,float b,float a)
    {
        Color newcolor = new Color(r, g, b, a);
        GetComponent<Renderer>().material.color = newcolor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}