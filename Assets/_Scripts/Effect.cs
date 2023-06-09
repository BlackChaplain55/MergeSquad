using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : ScriptableObject
{
    public EffectTypes EffectType;
    public string EffectName;
    public float EffectPower;

    public enum EffectTypes
    {
        HealOverTime,
        DamageOverTime,
        Weakness,
        Blessing
    }
}
