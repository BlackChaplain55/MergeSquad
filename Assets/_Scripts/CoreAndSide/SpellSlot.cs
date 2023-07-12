using DG.Tweening;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlot : EquipmentSlot
{
    public Action<MagicSO> OnMagicCast;
    public float CooldownTime
    {
        get { return _cooldownTime; }
        private set
        {
            _cooldownTime = value;
            OnPropertyChanged();
            ChangeBarValue();
        }
    }
    [SerializeField] private TextMeshProUGUI cooldownText;
    private float _maxCooldownTime;
    private float _cooldownTime;

    protected override void Awake()
    {
        base.Awake();
        OnItemChanged -= ChangeBorder;
        OnItemChanged -= ChangeUnderLayer;
        OnItemChanged += CastMagic;
    }

    protected void Update()
    {
        if (CooldownTime > 0)
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

        OnMagicCast?.Invoke(newItem as MagicSO);
        CooldownTime = _maxCooldownTime;
        SetItem(null);
    }

    public override void SetItem(ItemSO item)
    {
        if (item != null)
            _maxCooldownTime = ((MagicSO)item).CooldownTime;

        base.SetItem(item);
    }

    protected override void ChangeBarValue()
    {
        bool isUpdateNeeded = _cooldownTime > 0;

        bar.color = Color.white;
        barBackground.enabled = isUpdateNeeded;
        cooldownText.enabled = isUpdateNeeded;

        if (!isUpdateNeeded)
        {
            bar.DOColor(Color.clear, 0.6f).OnComplete(() => bar.enabled = isUpdateNeeded);
            return;
        }

        bar.enabled = isUpdateNeeded;
        float amount = _cooldownTime / _maxCooldownTime;
        barBackground.fillAmount = amount;
        bar.fillAmount = 1 - amount;
        bar.transform.rotation = Quaternion.Euler(0, 0, amount * 360);
        cooldownText.SetText(_cooldownTime.ToString("0.0"));
    }
}