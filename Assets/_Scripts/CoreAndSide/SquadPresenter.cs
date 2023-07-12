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
    }

    public void Sort(int sortOption)
    {
        IOrderedEnumerable<KeyValuePair<Unit, UnitPresenter>> orderedCollection;

        UnitSort param = (UnitSort)sortOption;

        switch (param)
        {
            case UnitSort.EquipDeathTimer:
                orderedCollection = UnitSlots.OrderBy(x => Math.Min(x.Value.Weapon.DeathTimer, x.Value.Armor.DeathTimer));
                break;
            case UnitSort.Level:
                orderedCollection = UnitSlots.OrderByDescending(x => x.Key.Level);
                break;
            case UnitSort.UnitType:
                int typesLength = Enum.GetValues(typeof(UnitType)).Length;
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
            }
        }
        if (e.OldItems != null)
        {
            foreach (Unit oldUnit in e.OldItems)
            {
                Remove(oldUnit);
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