using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnChance", menuName = "Scriptables/SpawnChance")]
public class ItemTypesSpawnChance : ScriptableObject
{
    [SerializedDictionary("ItemType", "Chance")] public SerializedDictionary<ItemType, int> ItemTypeChances;
    public int this[ItemType pointer] => ItemTypeChances[pointer];

    private void OnValidate()
    {
        if (ItemTypeChances != null && ItemTypeChances.Count > 0) return;

        ItemType[] types = (ItemType[])Enum.GetValues(typeof(ItemType));
        int typesCount = types.Length;
        ItemTypeChances = new SerializedDictionary<ItemType, int>(typesCount);

        for (int i = 0; i < typesCount; i++)
        {
            var itemType = types[i];
            ItemTypeChances.Add(itemType, 0);
        }
    }
}