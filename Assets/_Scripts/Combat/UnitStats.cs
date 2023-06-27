[System.Serializable]
public struct UnitStats : IUnitStatsProvider
{
    public UnitType Type { get; set; }
    public int MaxHealth { get; set; }
    public int HealthPerLevel { get; set; }
    public int Attack { get; set; }
    public int AttackPerLevel { get; set;}
    public int Level { get; set; }
    public int Health { get; set; }

    public UnitStats(int level, int health, UnitSO data)
    {
        Level = level;
        Health = health;
        Type = data.Type;
        MaxHealth = data.MaxHealth;
        HealthPerLevel = data.HealthPerLevel;
        Attack = data.Attack;
        AttackPerLevel = data.AttackPerLevel;
    }

    public float GetStats(UnitParameterType parameterType)
    {
        switch (parameterType)
        {
            case UnitParameterType.MaxHealth: return MaxHealth;
            case UnitParameterType.Attack: return Attack;
            case UnitParameterType.AttackPerLevel: return AttackPerLevel;
            case UnitParameterType.HealthPerLevel: return HealthPerLevel;
            default: return -1;
        }
    }
}