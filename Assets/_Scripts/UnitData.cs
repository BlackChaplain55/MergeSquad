using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit")]

public class UnitData : ScriptableObject
{
    [field: SerializeField] public float HP { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float AttackDistance { get; private set; }
    [field: SerializeField] public bool MultiAttack { get; private set; }
    [field: SerializeField] public bool RangedAttack { get; private set; }
    [field: SerializeField] public UnitTypes Type { get; private set; }
    [field: SerializeField] public int RespawnTime { get; private set; }
    [field: SerializeField] public float AttackSpeedModifier { get; private set; }
    [field: SerializeField] public float DamageModifier { get; private set; }
    [field: SerializeField] public float HPModifier { get; private set; }
}
