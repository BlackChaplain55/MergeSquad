using DG.Tweening;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

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
    private Hero _hero;

    protected override void Awake()
    {
        base.Awake();
        OnItemChanged -= ChangeBorder;
        OnItemChanged -= ChangeUnderLayer;
        OnItemChanged += CastMagic;
    }

    public void Init(Hero hero)
    {
        _hero = hero;
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
    }

    public override void SetItem(ItemSO item)
    {
        MagicSO magic = (MagicSO)item;
        if (item != null)
            _maxCooldownTime = magic.CooldownTime;

        _hero.SetMagic(magic);

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