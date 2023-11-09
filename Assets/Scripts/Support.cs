using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HappyHour/Create Support",fileName = "Support")]
public class Support : ScriptableObject
{
    public SupportType supportType;
    public GameObject supportPrefab;
    public Unit unit;
}
