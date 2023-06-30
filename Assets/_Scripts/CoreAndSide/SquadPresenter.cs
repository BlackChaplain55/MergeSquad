using UnityEngine;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;

public class SquadPresenter : MonoBehaviour
{
    [SerializeField] private GameObject unitPresenterSlotTemplate;
    [SerializeField] private GameObject unitPresenterParent;
    [SerializeField] private UnitController unitController;
    [SerializeField] private Dictionary<Unit, UnitPresenter> UnitSlots;

    private void OnValidate()
    {
        if (unitController == null)
            unitController = FindObjectOfType<UnitController>();
    }

    private void Start()
    {
        unitController.UnitsList.CollectionChanged += OnUnitsChanged;
        UnitSlots = new Dictionary<Unit, UnitPresenter>();
    }

    public void Add(Unit unit)
    {
        var unitGO = Instantiate(unitPresenterSlotTemplate, unitPresenterParent.transform);
        UnitPresenter presenter = unitGO.GetComponent<UnitPresenter>();

        presenter.SetUnit(unit);
        UnitSlots.Add(unit, presenter);
    }

    public void Remove(Unit unit)
    {
        Debug.LogWarning("Вырежь дестрой, замени на пул объектов");
        UnitSlots[unit].Clear();
        Destroy(UnitSlots[unit].gameObject);
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
}