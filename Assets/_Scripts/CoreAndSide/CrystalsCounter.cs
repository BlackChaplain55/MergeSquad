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
        GameController.Game.OnCrystalsChanged += UpdateText;
    }

    private void UpdateText(int crystalsValue)
    {
        textComponent.SetText(crystalsValue.ToString());
    }
}
