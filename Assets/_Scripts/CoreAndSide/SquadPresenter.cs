using UnityEngine;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

public class SquadPresenter : MonoBehaviour
{
    [SerializeField] private GameObject unitPresenterSlotTemplate;
    [SerializeField] private GameObject unitPresenterParent;
    [SerializeField] private MergeSystem mergeSystem;
    [SerializeField] private UnitController unitController;
    [SerializeField] private Dictionary<Unit, UnitPresenter> UnitSlots;
    private EquipmentBreaker _equipmentBreaker;
    private List<UnitType> _unitTypes;
    private int _typeSortCounter;

    private void OnValidate()
    {
        if (unitController == null)
            unitController = FindObjectOfType<UnitController>();
        if (mergeSystem == null)
            mergeSystem = FindObjectOfType<MergeSystem>();
    }

    private void Start()
    {
        unitController.UnitsList.CollectionChanged += OnUnitsChanged;
        UnitSlots = new Dictionary<Unit, UnitPresenter>();
        _equipmentBreaker = new(new List<EquipmentSlot>(), 0.02f);
        _equipmentBreaker.BreakRoutine = StartCoroutine(_equipmentBreaker.Tick());
        _unitTypes = new List<UnitType>();
    }

    public void Sort(int sortOption)
    {
        IOrderedEnumerable<KeyValuePair<Unit, UnitPresenter>> orderedCollection;

        UnitSort param = (UnitSort)sortOption;

        switch (param)
        {
            case UnitSort.EquipDeathTimer:
                orderedCollection = UnitSlots.OrderBy(x =>
                {
                    var weaponSlotTimer = x.Value.Weapon.DeathTimer;
                    var armorSlotTimer = x.Value.Armor.DeathTimer;
                    float weaponTimer = weaponSlotTimer > 0 ? weaponSlotTimer : float.MaxValue;
                    float armorTimer = armorSlotTimer > 0 ? armorSlotTimer : float.MaxValue;
                    return Math.Min(weaponTimer, armorTimer);
                });
                break;
            case UnitSort.Level:
                orderedCollection = UnitSlots.OrderByDescending(x => x.Key.Level);
                break;
            case UnitSort.UnitType:
                int typesLength = _unitTypes.Count;
                orderedCollection = UnitSlots.OrderBy(x => ((int)x.Key.UnitReadonlyData.Type + _typeSortCounter) % typesLength);
                _typeSortCounter++;
                break;
            case UnitSort.Health:
                orderedCollection = UnitSlots.OrderBy(x => x.Key.Health);
                break;
            case UnitSort.TraveledDistance:
                orderedCollection = UnitSlots.OrderBy(x => x.Key.transform.position.x);
                break;
            default:
                orderedCollection = (IOrderedEnumerable<KeyValuePair<Unit, UnitPresenter>>)UnitSlots;
                break;
        }

        var collection = orderedCollection.ToArray();

        for (int i =0; i < collection.Length; i++) 
            collection[i].Value.transform.SetSiblingIndex(i); 
    }

    public void Add(Unit unit)
    {
        var unitGO = Instantiate(unitPresenterSlotTemplate, unitPresenterParent.transform);
        UnitPresenter presenter = unitGO.GetComponent<UnitPresenter>();

        presenter.SetUnit(unit);
        UnitSlots.Add(unit, presenter);

        presenter.Weapon.OnItemOverlapChanged += mergeSystem.OverlapChanged;
        presenter.Armor.OnItemOverlapChanged += mergeSystem.OverlapChanged;
        mergeSystem.OnSlotCarryStateChanged += presenter.SetHighlightEquipment;
        mergeSystem.OnSlotCarryStateChanged += presenter.SetHighlightEquipment;

        _equipmentBreaker.Slots.Add(presenter.Weapon);
        _equipmentBreaker.Slots.Add(presenter.Armor);
    }

    public void Remove(Unit unit)
    {
        Debug.LogWarning("Вырежь дестрой, замени на пул объектов");
        var presenter = UnitSlots[unit];

        _equipmentBreaker.Slots.Remove(presenter.Weapon);
        _equipmentBreaker.Slots.Remove(presenter.Armor);

        presenter.Weapon.OnItemOverlapChanged -= mergeSystem.OverlapChanged;
        presenter.Armor.OnItemOverlapChanged -= mergeSystem.OverlapChanged;
        mergeSystem.OnSlotCarryStateChanged -= presenter.SetHighlightEquipment;
        mergeSystem.OnSlotCarryStateChanged -= presenter.SetHighlightEquipment;

        presenter.Clear();
        Destroy(presenter.gameObject);
        UnitSlots.Remove(unit);
    }

    private void OnUnitsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e == null) return;
        if (e.NewItems != null)
        {
            foreach (Unit newUnit in e.NewItems)
            {
                if (newUnit.UnitReadonlyData.Type == UnitType.Hero)
                    continue;

                Add(newUnit);

                var unitType = newUnit.UnitReadonlyData.Type;
                if (!_unitTypes.Contains(unitType))
                    _unitTypes.Add(unitType);
            }
        }
        if (e.OldItems != null)
        {
            foreach (Unit oldUnit in e.OldItems)
            {
                Remove(oldUnit);

                var unitType = oldUnit.UnitReadonlyData.Type;
                var units = UnitSlots.Keys.ToArray();
                if (units.First(x => x.UnitReadonlyData.Type == unitType) == null)
                    _unitTypes.Remove(unitType);
            }
        }
    }
}

public enum UnitSort
{
    EquipDeathTimer,
    Level,
    UnitType,
    Health,
    TraveledDistance
}