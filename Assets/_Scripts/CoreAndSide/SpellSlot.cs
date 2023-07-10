using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlot : EquipmentSlot, IPointerClickHandler
{
    [SerializeField] private MagicSystem magicSystem;

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

        magicSystem.CastMagic(CurrentItem.Type);
        SetItem(null);
    }

    protected override void ChangeBarValue() { }
}