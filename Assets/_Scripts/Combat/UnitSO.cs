using UnityEngine;

[System.Serializable]
public abstract class UnitSO : ScriptableObject, IUnitStatsProvider
{
    [field: SerializeField] public UnitType Type { get; private set; }
    [field: SerializeField] public int BaseAttack { get; private set; }
    [field: SerializeField] public int BaseHealth { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int AttackPerLevel { get; private set; }
    [field: SerializeField] public int HealthPerLevel { get; private set; }
    [field: SerializeField] public Sprite MainSprite { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    public int Level { get; protected set; }
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