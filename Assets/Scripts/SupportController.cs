using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SupportController : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private  SupportType type;
    [Range(0, 5)] public float radius;
    [SerializeField] private  Color gizmosColor;
    private CharacterManager mainController;
    private Wood targetWood;
    [SerializeField] private PhotonView PV;
    public Color GetColor { get; private set; }

    private void Start()
    {
        GetColor = GetComponent<Renderer>().material.color;
        //mainController = PhotonView.Find((int) PV.InstantiationData[0]).GetComponent<CharacterManager>();
    }

    private void Update()
    {
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