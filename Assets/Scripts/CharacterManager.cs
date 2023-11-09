using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public SupportType supportType;
    public Support[] Supports;
    public Vector3[] SpawnPoints;

    private void Start()
    {
        for (int i = 0; i < Supports.Length; i++)
        {
            Instantiate(Supports[i].supportPrefab, SpawnPoints[i], Quaternion.identity);
        }
    }


    private void OnGUI()
    {
        float boxWidth = 100f;
        float boxHeight = 50f;
        float boxHeightPadding = 100f;
        float leftMargin = 20f;
        float topMargin = (Screen.height - boxHeight) / 2;

        GUI.Box(new Rect(leftMargin, topMargin - boxHeightPadding, boxWidth, boxHeight), "Wood");

        float bottomMargin = 20f;
        float spacing = 10f;
        float buttonWidth = 80f;
        float buttonHeight = 30f;

        float firstButtonLeft = (Screen.width - 3 * (buttonWidth + spacing)) / 2;

        for (int i = 0; i < Supports.Length; i++)
        {
            float buttonLeft = firstButtonLeft + i * (buttonWidth + spacing);
            float buttonTop = Screen.height - buttonHeight - bottomMargin;
            var buttonStyle = (SupportType) i + 1 == supportType ? GetCustomButtonPressedStyle : GetCustomButtonStyle;
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

