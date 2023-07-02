using UnityEngine;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;

public class SquadPresenter : MonoBehaviour
{
    [SerializeField] private GameObject unitPresenterSlotTemplate;
    [SerializeField] private GameObject unitPresenterParent;
    [SerializeField] private MergeSystem mergeSystem;
    [SerializeField] private UnitController unitController;
    [SerializeField] private Dictionary<Unit, UnitPresenter> UnitSlots;
    private EquipmentBreaker _equipmentBreaker;

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

    public void Add(Unit unit)
    {
        var unitGO = Instantiate(unitPresenterSlotTemplate, unitPresenterParent.transform);
        UnitPresenter presenter = unitGO.GetComponent<UnitPresenter>();

        presenter.SetUnit(unit);
        UnitSlots.Add(unit, presenter);

        presenter.Weapon.OnItemOverlapChanged += OverlapChanged;
        presenter.Armor.OnItemOverlapChanged += OverlapChanged;

        _equipmentBreaker.Slots.Add(presenter.Weapon);
        _equipmentBreaker.Slots.Add(presenter.Armor);
    }

    public void Remove(Unit unit)
    {
        Debug.LogWarning("Вырежь дестрой, замени на пул объектов");
        var presenter = UnitSlots[unit];

        _equipmentBreaker.Slots.Remove(presenter.Weapon);
        _equipmentBreaker.Slots.Remove(presenter.Armor);

        presenter.Weapon.OnItemOverlapChanged -= OverlapChanged;
        presenter.Armor.OnItemOverlapChanged -= OverlapChanged;

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

    private void OverlapChanged(Slot slot, bool state)
    {
        if (state)
            mergeSystem.OnHover(slot);
        else
            mergeSystem.OnHoverEnd(slot);
    }
}