using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour,IPickable
{
    public void Pick(CharacterManager characterManager)
    {
        characterManager.IncreaseWoodCount();
        Destroy(gameObject);
    }
}
