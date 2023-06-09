using UnityEngine;
using System.Collections.Generic;

public static class MergeData
{
    public static Dictionary<ItemTypes, ItemResources> itemsDictionary = new Dictionary<ItemTypes, ItemResources>();
    public static List<CharacterSO> Enemies = new List<CharacterSO>();

    public static void InitResources()
    {
        foreach (ItemTypes type in MergeConroller.TypesList)
        {
            ItemResources gameResources = Resources.Load<ItemResources>("Items/"+type.ToString());
            itemsDictionary.Add(type, gameResources);
        };
        Enemies.AddRange(Resources.LoadAll<CharacterSO>("Enemies"));
    }

    public static Sprite GetItemVisualById(ItemTypes type,int itemId)
    {
        ItemResources currentResourses = itemsDictionary.GetValueOrDefault(type);
        Sprite currentVisual = currentResourses.items[itemId].ItemSprite;
        return currentVisual;
    }

    public static string GetShortName(ItemTypes type)
    {
        ItemResources currentResourses = itemsDictionary.GetValueOrDefault(type);
        string text = currentResourses.ShortName;
        return text;
    }

    public static string GetFullName(ItemTypes type)
    {
        ItemResources currentResourses = itemsDictionary.GetValueOrDefault(type);
        string text = currentResourses.FullName;
        return text;
    }

    public enum ItemTypes
    {
        Armour,
        Sword,
        Bow,
        Knife,
        Mace,
        Staff,
        WarriorAbility,
        RangerAbility,
        RogueAbility,
        WizardAbility,
        ClericAbility,
        Dummy
    }
}