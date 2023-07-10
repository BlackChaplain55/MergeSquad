using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public struct UnitStats : INotifyPropertyChanged
{
    public UnitType Type { get; set; }
    public int Attack
    {
        get { return _attack; }
        set
        {
            _attack = value;
            OnPropertyChanged();
        }
    }
    public int MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;
            OnPropertyChanged();
        }
    }
    public float AttackSpeed
    {
        get { return _attackSpeed; }
        set
        {
            _attackSpeed = value;
            OnPropertyChanged();
        }
    }
    public int UpgradeCost
    {
        get { return _upgradeCost; }
        set
        {
            _upgradeCost = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private int _attack;
    private int _maxHealth;
    private float _attackSpeed;
    private int _upgradeCost;

    public UnitStats(UnitData data)
    {
        Type = data.Type;
        _attackSpeed = data.AttackSpeed;
        _attack = 0;
        _maxHealth = 0;
        _upgradeCost = 0;
        PropertyChanged = null;
    }


    public void SetSnapshot(IUnitStatsProvider statsProvider)
    {
        Attack = (int)statsProvider.GetStats(UnitParameterType.Attack);
        AttackSpeed = statsProvider.GetStats(UnitParameterType.AttackSpeed);
        MaxHealth = (int)statsProvider.GetStats(UnitParameterType.MaxHealth);
        UpgradeCost = (int)statsProvider.GetStats(UnitParameterType.UpgradeCost);
    }

    public void ReCompute(UnitParameterType parameterType, IUnitStatsProvider statsProvider)
    {
        switch (parameterType)
        {
            case UnitParameterType.Attack: Attack = (int)statsProvider.GetStats(parameterType); break;
            case UnitParameterType.AttackSpeed: AttackSpeed = (int)statsProvider.GetStats(parameterType); break;
            case UnitParameterType.MaxHealth: MaxHealth = (int)statsProvider.GetStats(parameterType); break;
            case UnitParameterType.UpgradeCost: UpgradeCost = (int)statsProvider.GetStats(parameterType); break;
            default: break;
        }
    }

    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}