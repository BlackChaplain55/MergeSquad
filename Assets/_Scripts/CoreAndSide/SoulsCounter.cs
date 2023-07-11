using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;

    private void OnValidate()
    {
        if (textComponent == null)
            textComponent = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Awake()
    {
        GameController.Game.OnSoulsChanged += UpdateText;
    }

    private void UpdateText(int soulsValue)
    {
        textComponent.SetText(soulsValue.ToString());
    }
}
