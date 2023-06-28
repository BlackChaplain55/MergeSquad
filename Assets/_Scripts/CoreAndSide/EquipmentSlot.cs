using UnityEngine;

public class EquipmentSlot : Slot
{
    public CharController Char;
    public ItemType ItemType;

    public override bool TryPlace(Slot slot)
    {
        return ItemType == slot.CurrentItem.Type && CurrentItem.Id < slot.CurrentItem.Id;
    }
}
