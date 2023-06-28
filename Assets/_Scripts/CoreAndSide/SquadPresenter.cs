using UnityEngine;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;

public class SquadPresenter : MonoBehaviour
{
    [SerializeField] private EquipmentSlot equipmentSlotTemplate;
    [SerializeField] private List<UnitPresenter> UnitSlots;
    [SerializeField] private Squad squad;
    public int EmptySlots { get; private set; }

    private void Awake()
    {
        squad.Units.CollectionChanged += OnUnitsChanged;
    }

    public void Add(Unit unit)
    {
        if (EmptySlots <= 0)
        { return; }

        EmptySlots--;
        for (int i = 0; i < UnitSlots.Count; i++)
        {
            if (UnitSlots[i].Unit == null)
            {
                UnitSlots[i].SetUnit(unit);
                break;
            }
        }
    }

    public void Remove(Unit unit)
    {
        EmptySlots++;
        foreach (var slot in UnitSlots)
        {
            if (slot.Unit == unit)
            {
                slot.Clear();
                break;
            }
        }
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