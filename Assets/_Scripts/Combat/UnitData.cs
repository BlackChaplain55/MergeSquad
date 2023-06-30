using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="NewUnit", menuName ="Scriptables/Unit")]
public class UnitData : ScriptableObject, IUnitStatsProvider
{
    [field: SerializeField] public UnitType Type { get; private set; }
    [field: SerializeField] public int BaseHealth { get; private set; }
    [field: SerializeField] public int BaseAttack { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public float AttackSpeedPerLevel { get; private set; }
    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float AttackDistance { get; private set; }
    [field: SerializeField] public int SummonCost { get; private set; }
    [field: SerializeField] public int BaseUpgradeCost { get; private set; }
    [field: SerializeField] public float UpgradeCostPerLevel { get; private set; }
    [field: SerializeField] public bool MultiAttack { get; private set; }
    [field: SerializeField] public int RespawnTime { get; private set; }
    [field: SerializeField] public float AttackPerLevel { get; private set; }
    [field: SerializeField] public float HealthPerLevel { get; private set; }
    [field: SerializeField] public Sprite MainSprite { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public ItemType ArmorType { get; private set; }
    [field: SerializeField] public ItemType WeaponType { get; private set; }

    public float GetStats(UnitParameterType parameterType)
    {
        switch (parameterType)
        {
            case UnitParameterType.BaseAttackSpeed: return AttackSpeed;
            case UnitParameterType.AttackSpeedPerLevel: return AttackSpeedPerLevel;
            case UnitParameterType.BaseAttack: return BaseAttack;
            case UnitParameterType.BaseHealth: return BaseHealth;
            case UnitParameterType.AttackDistance: return AttackDistance;
            case UnitParameterType.WalkSpeed: return WalkSpeed;
            case UnitParameterType.SummonCost: return SummonCost;
            case UnitParameterType.BaseUpgradeCost: return BaseUpgradeCost;
            case UnitParameterType.UpgradeCostPerLevel: return UpgradeCostPerLevel;
            case UnitParameterType.RespawnTime: return RespawnTime;
            case UnitParameterType.AttackPerLevel: return AttackPerLevel;
            case UnitParameterType.HealthPerLevel: return HealthPerLevel;
            default: return -1;
        }
    }
}