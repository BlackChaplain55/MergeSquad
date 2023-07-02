using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitPresenter : MonoBehaviour
{
    public Unit Unit { get; private set; }
    public Sprite UnitSprite { get; private set; }
    public EquipmentSlot Weapon { get { return weapon; } }
    public EquipmentSlot Armor { get { return armor; } }
    public event UnityAction OnUpgradeRequest;
    public event Action<EquipmentSO> OnWeaponChanged;
    public event Action<EquipmentSO> OnArmorChanged;
    [SerializeField] private List<GameObject> elements;
    [SerializeField] private EquipmentSlot weapon;
    [SerializeField] private EquipmentSlot armor;
    [SerializeField] private Image deathTimerSlider;
    [SerializeField] private Image deathTimerPallete;
    [SerializeField] private Image unitIcon;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI attack;

    private void Awake()
    {
        if (upgradeButton != null) upgradeButton.onClick.AddListener(() => OnUpgradeRequest?.Invoke());
        OnWeaponChanged += eqs => Weapon.GetItemTransform().localPosition = Vector3.zero;
        OnArmorChanged += eqs => Armor.GetItemTransform().localPosition = Vector3.zero;
    }

    public void SetUnit(Unit unit)
    {
        Unit = unit;
        Activate();
        Unit.PropertyChanged += UpdateText;
        Unit.GetUnitStatsRef().PropertyChanged += UpdateText;
        weapon.ItemType = Unit.UnitReadonlyData.WeaponType;
        armor.ItemType = Unit.UnitReadonlyData.ArmorType;
    }

    public void Activate()
    {
        ToggleActive(true);

        if (unitIcon != null) unitIcon.sprite = Unit.UnitReadonlyData.MainSprite;
        if (level != null) attack.text = Unit.Level.ToString();
        if (attack != null) attack.text = Unit.UnitStats.Attack.ToString();
        if (health != null) health.text = Mathf.Ceil(Unit.Health).ToString();
        if (upgradeCost != null) upgradeCost.text = Unit.UnitStats.UpgradeCost.ToString();
        OnUpgradeRequest += Unit.Upgrade;

        weapon.OnItemReceived += Weapon_OnItemReceived;
        armor.OnItemReceived += Armor_OnItemReceived;
        weapon.PropertyChanged += (s, e) => EquipmentPropertyChanged(weapon, e.PropertyName);
        armor.PropertyChanged += (s, e) => EquipmentPropertyChanged(armor, e.PropertyName);
    }

    private void EquipmentPropertyChanged(EquipmentSlot slot, string propertyName)
    {
        if (propertyName != "DeathTimer") return;

        Color hundred = Color.HSVToRGB(1/3f, 0.7f, 1);
        Color zero = Color.HSVToRGB(0, 0.7f, 1);
        var eqData = (EquipmentSO)slot.CurrentItem;
        float amount = slot.DeathTimer / eqData.DeathTimer;

        if (slot == weapon)
        {
            deathTimerSlider.fillAmount = amount;
            deathTimerSlider.color = Color.Lerp(zero, hundred, amount);
        }
        if (slot == armor)
        {
            deathTimerPallete.color = Color.Lerp(zero, hundred, amount);
        }
    }

    private void Weapon_OnItemReceived(ItemSO item)
    {
        if (item is EquipmentSO equipment)
        {
            Unit?.SetWeapon(equipment);
            weapon.ResetDeathTimer(equipment);
        }
    }

    private void Armor_OnItemReceived(ItemSO item)
    {
        if (item is EquipmentSO equipment)
        {
            Unit?.SetArmor(equipment);
            armor.ResetDeathTimer(equipment);
        }
    }

    public void Clear()
    {
        ToggleActive(false);
        Unit.PropertyChanged -= UpdateText;
        weapon.OnItemReceived -= Weapon_OnItemReceived;
        armor.OnItemReceived -= Armor_OnItemReceived;
        OnUpgradeRequest -= Unit.Upgrade;
    }

    public void UpdateText(object sender, PropertyChangedEventArgs e)
    {
        Console.WriteLine("Prop: " + e.PropertyName);
        switch (e.PropertyName)
        {
            case nameof(UnitParameterType.Level): if (level != null) level.text = Unit.Level.ToString();
                break;
            case nameof(UnitParameterType.Attack): if (attack != null) attack.text = Unit.UnitStats.Attack.ToString();
                break;
            case nameof(UnitParameterType.Health): if (health != null) health.text = Unit.Health.ToString();
                break;
            case nameof(UnitParameterType.UpgradeCost): if (upgradeCost != null) upgradeCost.text = Unit.UnitStats.UpgradeCost.ToString();
                break;
            default:
                break;
        }
    }

    private void ToggleActive(bool flag)
    {
        if (elements == null) return;

        foreach (var el in elements)
        {
            el.SetActive(flag);
        }
    }
}
