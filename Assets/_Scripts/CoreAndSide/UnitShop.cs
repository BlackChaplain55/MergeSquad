using TMPro;
using UnityEngine;

public class UnitShop : MonoBehaviour
{
    public int Souls
    {
        get { return _souls; }
        private set
        {
            _souls = value;
            soulsText.text = value.ToString();
        }
    }
    [SerializeField] private UnitSpawner spawner;
    [SerializeField] private GameObject summonButtonTemplate;
    [SerializeField] private Transform summonButtonsParent;
    [SerializeField] private TextMeshProUGUI soulsText;
    private int _souls;

    private void Awake()
    {
        var unitTemplates = spawner._unitTemplatesDictionary;
        Souls = GameController.Game.Settings.StartSouls;
        EventBus.onUnitDeath += unit => { if (unit.isEnemy) Souls += 5; };

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
        int remainSouls = Souls - unitData.SummonCost;
        if (remainSouls < 0)
            return;

        spawner.Spawn(template);
        Souls = remainSouls;
    }
}