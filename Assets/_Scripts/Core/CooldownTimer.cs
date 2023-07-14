using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CooldownTimer : MonoBehaviour
{
    [SerializeField] private Image lighten;
    [SerializeField] private Image darken;
    [SerializeField] private TextMeshProUGUI textComponent;

    public void UpdateValue(float value, float maxValue, Func<bool> p_isUpdateNeeded)
    {
        bool isUpdateNeeded = p_isUpdateNeeded();

        lighten.color = Color.white;
        darken.enabled = isUpdateNeeded;
        textComponent.enabled = isUpdateNeeded;

        if (!isUpdateNeeded)
        {
            lighten.DOColor(Color.clear, 0.6f).OnComplete(() => lighten.enabled = isUpdateNeeded);
            return;
        }

        lighten.enabled = isUpdateNeeded;
        float amount = value / maxValue;
        darken.fillAmount = amount;
        lighten.fillAmount = 1 - amount;
        lighten.transform.rotation = Quaternion.Euler(0, 0, amount * 360);
        textComponent.SetText(value.ToString("0.0"));
    }
}
