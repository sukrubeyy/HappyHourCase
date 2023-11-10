using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private void Start()
    {
        CreateSupportObjects();
    }

    private void CreateSupportObjects()
    {
        supportsController = new SupportController[Supports.Length];
        for (int i = 0; i < Supports.Length; i++)
        {
            supportsController[i] = Instantiate(Supports[i].supportPrefab, SpawnPoints[i], Quaternion.identity).GetComponent<SupportController>();
            supportsController[i].SetController(this);
        }
    }

    public void IncreaseWoodCount() => woodCount++;
    private void Update()
    {
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
                        var wood = hit.transform.GetComponent<Wood>();
                        supportsController.FirstOrDefault(x => x.type == supportType).SetTarget(wood.transform.position,wood);
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
            var buttonStyle = (SupportType)i + 1 == supportType ? GetCustomButtonPressedStyle : GetCustomButtonStyle;

            if (GUI.Button(new Rect(buttonLeft, buttonTop, buttonWidth, buttonHeight), $"{Supports[i].supportType}", buttonStyle))
            {
                if (supportType == (SupportType) i + 1)
                    supportType = SupportType.None;
                else
                    supportType = (SupportType) i + 1;
            }
        }
    }

    private GUIStyle GetCustomButtonStyle => new GUIStyle(GUI.skin.button)
    {
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState
        {
            background = Texture2D.whiteTexture,
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

    private GUIStyle GetCustomButtonPressedStyle => new GUIStyle(GUI.skin.button)
    {
        alignment = TextAnchor.MiddleCenter,
        normal = new GUIStyleState
        {
            background = Texture2D.grayTexture,
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
}