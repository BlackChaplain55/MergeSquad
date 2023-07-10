using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlot : EquipmentSlot, IPointerClickHandler
{
    public Action<ItemSO> OnMagicCast;

    public override bool TryPlace(Slot slot)
    {
        bool isMagic = GameController.Game.Settings.MagicTypes.Contains(slot.CurrentItem.Type);
        bool isLowerLevel = true;
        if (CurrentItem != null)
            isLowerLevel = slot.CurrentItem.Id >= CurrentItem.Id;

        return isMagic && isLowerLevel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentItem == null) return;

        OnMagicCast?.Invoke(CurrentItem);
        SetItem(null);
    }

    protected override void ChangeBarValue() { }
}