using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonButton : MonoBehaviour
{
    [field: SerializeField] public Image UnitLabel { get; private set; }
    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public TextMeshProUGUI SummonCost { get; private set; }
    private UnitType _unitType;

    public void SetUnit(UnitData data, int summonCost)
    {
        _unitType = data.Type;
        UnitLabel.sprite = data.Icon;
        SetSummonCost(summonCost);
    }

    public void SetSummonCost(UnitType unitType, int value)
    {
        if (_unitType == unitType)
            SetSummonCost(value);
    }

    public void SetSummonCost(int value)
    {
        SummonCost.text = value.ToString();
    }
}