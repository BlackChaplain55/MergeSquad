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
    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float AttackDistance { get; private set; }
    [field: SerializeField] public bool MultiAttack { get; private set; }
    [field: SerializeField] public int RespawnTime { get; private set; }
    [field: SerializeField] public int AttackPerLevel { get; private set; }
    [field: SerializeField] public int HealthPerLevel { get; private set; }
    [field: SerializeField] public Sprite MainSprite { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    public int Level { get; protected set; }
    public int Attack { get; }
    public int MaxHealth { get; }
    
    public float GetStats(UnitParameterType parameterType)
    {
        switch (parameterType)
        {
            case UnitParameterType.Attack: return Attack;
            case UnitParameterType.MaxHealth: return MaxHealth;
            case UnitParameterType.AttackPerLevel: return AttackPerLevel;
            case UnitParameterType.HealthPerLevel: return HealthPerLevel;
            case UnitParameterType.BaseAttack: return BaseAttack;
            case UnitParameterType.BaseHealth: return BaseHealth;
            default: return -1;
        }
    }
}