using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CharacterManager : MonoBehaviour
{
    public SupportType supportType;
    public Support[] Supports;
    public Vector3[] SpawnPoints;
    public SupportController[] supportsController;
    private int woodCount;
    private PhotonView PV;
    private Wood selectedWood;
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            SetPosition();
            CreateSupportObjects();
        }
    }

    private void SetPosition()
    {
        transform.position = SpawnManager.Instance.GetSpawnPoint().position;
        transform.rotation = SpawnManager.Instance.GetSpawnPoint().rotation;
    }

    private void CreateSupportObjects()
    {
        supportsController = new SupportController[Supports.Length];
        float padding = 5f;
        for (int i = 0; i < Supports.Length; i++)
        {
            var spawnPos = transform.position + (transform.right * (i - 1) * padding) ;
            supportsController[i] = PhotonNetwork.Instantiate(System.IO.Path.Combine("PhotonPrefabs", Supports[i].name), spawnPos,  Quaternion.identity, 0,new object[] { PV.ViewID}).GetComponent<SupportController>();
            supportsController[i].SetController(this);
        }
    }

    public void IncreaseWoodCount()
    {
        woodCount++;  
    }
   
    private void Update()
    {
        if(!PV.IsMine)return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (supportType is not SupportType.None)
                {
                    var pickableObject = hit.transform.GetComponent<IPickable>();
                    if (pickableObject is not null)
                    {
                        selectedWood = hit.transform.GetComponent<Wood>();
                        supportsController.FirstOrDefault(x => x.type == supportType).SetTarget(selectedWood.transform.position,selectedWood);
                    }
                    else
                    {
                        supportsController.FirstOrDefault(x => x.type == supportType).SetTarget(hit.point);
                    }
                }
                else
                {
                    Debug.LogWarning("Lütfen Support Seç");
                }
            }
        }
    }

    private void OnGUI()
    {
        if(!PV.IsMine)return;

        float boxWidth = 100f;
        float boxHeight = 50f;
        float boxHeightPadding = 100f;
        float leftMargin = 20f;
        float topMargin = (Screen.height - boxHeight) / 2;

        GUI.Box(new Rect(leftMargin, topMargin - boxHeightPadding, boxWidth, boxHeight), $"Wood {woodCount}");

        float spacing = 10f;
        float buttonWidth = 80f;
        float buttonHeight = 30f;

        for (int i = 0; i < 3; i++)
        {
            float buttonLeft = leftMargin;
            float buttonTop = topMargin + boxHeightPadding + i * (buttonHeight + spacing); 
            var buttonStyle = (SupportType)i + 1 == supportType ? GetCustomButtonPressedStyle(i) : GetCustomButtonStyle(i);
            if (GUI.Button(new Rect(buttonLeft, buttonTop, buttonWidth, buttonHeight), $"{Supports[i].supportType}", buttonStyle))
            {
                if (supportType == (SupportType) i + 1)
                    supportType = SupportType.None;
                else
                    supportType = (SupportType) i + 1;
            }
        }
    }

    private GUIStyle GetCustomButtonStyle(int index) => new GUIStyle(GUI.skin.button)
    {
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState
        {
            background = ColorToTexture(supportsController[index].GetColor),
            textColor = Color.black
        },
        active = new GUIStyleState
        {
            background = Texture2D.grayTexture,
            textColor = Color.white
        },
        hover = new GUIStyleState
        {
            background = Texture2D.grayTexture,
            textColor = Color.white
        },
        fontSize = 10,
        fontStyle = FontStyle.Bold
    };

    private GUIStyle GetCustomButtonPressedStyle(int index) => new GUIStyle(GUI.skin.button)
    {
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState
        {
            background = ColorToTexture(supportsController[index].GetColor),
            textColor = Color.white
        },
        active = new GUIStyleState
        {
            background = Texture2D.whiteTexture,
            textColor = Color.black
        },
        hover = new GUIStyleState
        {
            background = Texture2D.whiteTexture,
            textColor = Color.black
        },
        fontSize = 10,
        fontStyle = FontStyle.Bold
    };
    
    private Texture2D ColorToTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }
}