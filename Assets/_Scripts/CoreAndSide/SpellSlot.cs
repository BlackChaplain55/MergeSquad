using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlot : EquipmentSlot
{
    public Action<ItemSO> OnMagicCast;
    public float CooldownTime;

    protected override void Awake()
    {
        base.Awake();
        OnItemReceived += CastMagic;
    }

    protected void Update()
    {
        CooldownTime -= Time.deltaTime;
    }

    public override bool TryPlace(Slot slot)
    {
        bool isNotOnCooldown = CooldownTime <= 0;
        bool isMagic = GameController.Game.Settings.MagicTypes.Contains(slot.CurrentItem.Type);

        return isMagic && isNotOnCooldown;
    }

    public void CastMagic(ItemSO newItem)
    {
        if (newItem == null) return;

        OnMagicCast?.Invoke(newItem);
        SetItem(null);
    }

    protected override void ChangeBarValue() { }
}