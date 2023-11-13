using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text nickText;

    public void Initialize(string nick)
    {
        nickText.text = nick;
    }
}