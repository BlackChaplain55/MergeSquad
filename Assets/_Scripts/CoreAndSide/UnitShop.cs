using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitShop : MonoBehaviour
{
    [SerializeField] private UnitSpawner spawner;
    [SerializeField] private GameObject summonButtonTemplate;
    [SerializeField] private Transform summonButtonsParent;
    public Action<UnitType, int> OnUnitsCostChanged;
    private Dictionary<UnitType, SummonCost> _unitTypeCost;

    private void Awake()
    {
        _unitTypeCost = new();

        var unitTemplates = spawner._unitTemplatesDictionary;

        foreach (var unitType in unitTemplates)
        {
            var unitData = unitType.Value.GetComponent<Unit>().UnitReadonlyData;
            var summonGO = Instantiate(summonButtonTemplate, summonButtonsParent);
            var summonButton = summonGO.GetComponent<SummonButton>();
            summonButton.SetUnit(unitData, unitData.SummonCost);
            summonButton.Button.onClick.AddListener(() => TryBuyNewUnit(unitData, unitTemplates[unitType.Key]));

            OnUnitsCostChanged += summonButton.SetSummonCost;
            
            if (!_unitTypeCost.ContainsKey(unitType.Key))
                _unitTypeCost.Add(unitType.Key, new(1, 1));
        }
    }

    private void TryBuyNewUnit(UnitData unitData, GameObject template)
    {
        int summonCost = (int)(unitData.SummonCost * (_unitTypeCost[unitData.Type].Multiplier));
        SummonCost cost = _unitTypeCost[unitData.Type];
        float summonProgression = GameController.Game.Settings.UnitSummonCostProgression;

        if (GameController.Game.TrySpendSouls(summonCost))
        {
            spawner.Spawn(template);

            int unitsCount = ++cost.UnitsCount;
            float newMultiplier = Mathf.Pow(summonProgression, unitsCount);
            _unitTypeCost[unitData.Type] = new (unitsCount,
                newMultiplier);

            OnUnitsCostChanged?.Invoke(unitData.Type, (int)(unitData.SummonCost * newMultiplier));
        }
    }

    private struct SummonCost
    {
        public int UnitsCount;
        public float Multiplier;

        public SummonCost(int unitsCount = 1, float multiplier = 1)
        {
            UnitsCount = unitsCount;
            Multiplier = multiplier;
        }
    }
}