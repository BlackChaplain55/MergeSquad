using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot
{
    public CharController Char;
    public ItemType ItemType;

    public override bool TryPlace(Slot slot)
    {
        return ItemType == slot.CurrentItem.Type && CurrentItem.Id < slot.CurrentItem.Id;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {}

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("HOVER!");
    }
}
