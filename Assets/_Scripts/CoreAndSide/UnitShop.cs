using TMPro;
using UnityEngine;

public class UnitShop : MonoBehaviour
{
    [SerializeField] private UnitSpawner spawner;
    [SerializeField] private GameObject summonButtonTemplate;
    [SerializeField] private Transform summonButtonsParent;

    private void Awake()
    {
        var unitTemplates = spawner._unitTemplatesDictionary;

        foreach (var unitType in unitTemplates)
        {
            var unitData = unitType.Value.GetComponent<Unit>().UnitReadonlyData;
            var summonGO = Instantiate(summonButtonTemplate, summonButtonsParent);
            var summonButton = summonGO.GetComponent<SummonButton>();
            summonButton.SetUnit(unitData, unitData.SummonCost);
            summonButton.Button.onClick.AddListener(() => TryBuyNewUnit(unitData, unitTemplates[unitType.Key]));
        }
    }

    private void TryBuyNewUnit(UnitData unitData, GameObject template)
    {
        if (GameController.Game.SpendSouls(unitData.SummonCost))
            spawner.Spawn(template);
    }
}