using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot
{
    public float DeathTimer
    {
        get { return _deathTimer; }
        set
        {
            _deathTimer = value;
            OnPropertyChanged();
        }
    }
    public ItemType ItemType;
    private float _deathTimer;

    public void ResetDeathTimer(EquipmentSO newItem)
    {
        DeathTimer = newItem.DeathTimer;
    }

    public override bool TryPlace(Slot slot)
    {
        bool isSameType = ItemType == slot.CurrentItem.Type;
        bool isLowerLevel = true;
        if (CurrentItem != null)
            isLowerLevel = slot.CurrentItem.Id >= CurrentItem.Id;

        return isSameType && isLowerLevel;
    }
}
