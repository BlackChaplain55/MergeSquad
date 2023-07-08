using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealtBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _healthBarBG;
    [SerializeField] private float _healthScaleX;
    [SerializeField] private float _healthSizeY;

    public void Init(Vector2 size)
    {
        //_healthBarBG.rectTransform.localPosition = new Vector2(0, size.y / 2);
        _healthBarBG.rectTransform.sizeDelta = new Vector2( size.x* _healthScaleX, _healthSizeY);      
        _healthBar.fillAmount = 1;
    }

    public void SetHealthBar(float amount)
    {
        amount = Mathf.Clamp(amount, 0, 1);
        _healthBar.fillAmount = amount;
    }
}
