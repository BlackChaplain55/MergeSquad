using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill")]

public class Skill : ScriptableObject
{
    public SkillTypes SkillType;
    public string SkillName;
    public float Effect;
    public float CriticalChance;

    public enum SkillTypes
    {
        Damage,
        AreaDamage,
        Armour,
        Heal,
        AreaHeal,
        DamageOverTime,
        Weakness,
        Blessing
    }
}
