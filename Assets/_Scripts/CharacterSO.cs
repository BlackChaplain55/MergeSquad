using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]

public class CharacterSO : ScriptableObject
{
    public int ArmourID;
    public int WeaponID;
    public string CharName;
    public Skill AttackSkill;
    public Skill DefenceSkill;
    public int HP;
    public MergeData.ItemTypes[] WeaponType;
    public MergeData.ItemTypes[] SkillType;
    public Sprite visual;
}
