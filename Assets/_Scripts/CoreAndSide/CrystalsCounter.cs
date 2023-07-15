using System.ComponentModel;
using TMPro;
using UnityEngine;

public class CrystalsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;

    private void OnValidate()
    {
        if (textComponent == null)
            textComponent = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Awake()
    {
        GameController.Game.GameProgress.PropertyChanged += UpdateText;
        SetText();
    }

    private void UpdateText(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GameProgress.Crystals))
            SetText();
    }

    private void SetText()
    {
        textComponent.SetText(GameController.Game.GameProgress.Crystals.ToString());
    }
}
