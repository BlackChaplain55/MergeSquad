using System;
using UnityEngine;
using System.Collections.Generic;

public static class MergeData
{
    public static Dictionary<ItemTypes, ItemResources> itemsDictionary = new Dictionary<ItemTypes, ItemResources>();

    public static void InitResources()
    {
        Array types = Enum.GetValues(typeof(ItemTypes));
        foreach (int i in types)
        {
            var type = (ItemTypes)i;
            ItemResources gameResources = Resources.Load<ItemResources>("Items/"+ type.ToString());
            itemsDictionary.Add(type, gameResources);
        };
    }

    public static Sprite GetItemVisualById(ItemTypes type, int itemId) => 
        itemsDictionary.GetValueOrDefault(type).items[itemId].ItemSprite;

    public static string GetShortName(ItemTypes type) => itemsDictionary.GetValueOrDefault(type).ShortName;
    public static string GetFullName(ItemTypes type) => itemsDictionary.GetValueOrDefault(type).FullName;
}