using System;
using UnityEngine;
using System.Collections.Generic;

public static class MergeData
{
    public static Dictionary<ItemType, ItemResources> itemsDictionary = new Dictionary<ItemType, ItemResources>();

    public static void InitResources()
    {
        Array types = Enum.GetValues(typeof(ItemType));
        foreach (int i in types)
        {
            var type = (ItemType)i;
            ItemResources gameResources = Resources.Load<ItemResources>("Items/"+ type.ToString());
            itemsDictionary.Add(type, gameResources);
        };

        Debug.Log(itemsDictionary[ItemType.CurseMagic].GetItem(0).Type.ToString());
    }

    public static Sprite GetItemVisualById(ItemType type, int itemId) => 
        itemsDictionary.GetValueOrDefault(type).items[itemId].ItemSprite;

    public static string GetShortName(ItemType type) => itemsDictionary.GetValueOrDefault(type).ShortName;
    public static string GetFullName(ItemType type) => itemsDictionary.GetValueOrDefault(type).FullName;
}