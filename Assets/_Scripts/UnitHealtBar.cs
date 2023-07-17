using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealtBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _healthBarBG;
    [SerializeField] private float _healthScaleX;
    [SerializeField] private float _healthSizeY;
    [SerializeField] private Image _levelBG;
    [SerializeField] private TextMeshProUGUI _levelText;

    public void Init(Vector2 size)
    {
        //_healthBarBG.rectTransform.localPosition = new Vector2(0, size.y / 2);
        _healthBarBG.rectTransform.sizeDelta = new Vector2( size.x* _healthScaleX, _healthSizeY);
        _levelBG.rectTransform.sizeDelta = new Vector2(size.x * _healthScaleX / 4, _healthSizeY * 3);
        _levelBG.rectTransform.SetLocalPositionAndRotation(new Vector3(-size.x * _healthScaleX * 0.6f, 0, 0), Quaternion.identity);
        _healthBar.fillAmount = 1;
    }

    public void SetHealthBar(float amount)
    {
        amount = Mathf.Clamp(amount, 0, 1);
        _healthBar.fillAmount = amount;
    }

    public void SetLevel(int value)
    {
        _levelText.SetText($"{value}"); //<size=0.45em>LVL</size>");
    }
}
