using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomInfoBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] texts = null;

    private void Awake()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void SetRoom(string _roomName)
    {
        texts[1].text = "1/1";
        texts[3].text = _roomName;
    }
}
