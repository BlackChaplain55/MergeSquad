using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class MergebleItemsSpawner : MonoBehaviour
{
    [SerializedDictionary("ItemType", "Chance")] public SerializedDictionary<ItemTypes, int> ItemTypeChances;

    private int _totalItemChances = 0;
    private SlotSpawner _slotSpawner;

    private void OnValidate()
    {
        if (ItemTypeChances != null && ItemTypeChances.Count > 0) return;

        ItemTypes[] types = (ItemTypes[])Enum.GetValues(typeof(ItemTypes));
        int typesCount = types.Length;
        ItemTypeChances = new SerializedDictionary<ItemTypes, int>(typesCount);

        for (int i = 0; i < typesCount; i++)
        {
            var itemType = types[i];
            ItemTypeChances.Add(itemType, 0);
        }
    }

    private void Awake()
    {
        MergeData.InitResources();
        InitChancesDictionary();
        _slotSpawner = GetComponent<SlotSpawner>();
    }

    public void PlaceRandomItem()
    {
        var emptySlots = FindEmptySlotsIndex();
        for (int i = 1; i <= GameController.Game.Settings.ItemsPerRound; i++)
        {
            if (emptySlots.Count < 1)
            {
                return;
            }
            int itemRandom = UnityEngine.Random.Range(0, _totalItemChances);
            int currentChance = 0;
            int currentChanceAccum = 0;

            int typeIndex = 0;
            Array itemTypes = Enum.GetValues(typeof(ItemTypes));

            for (; typeIndex < itemTypes.Length; typeIndex++)
            {
                currentChance = ItemTypeChances.GetValueOrDefault((ItemTypes)typeIndex);
                currentChanceAccum += currentChance;
                if (itemRandom <= currentChanceAccum)
                {
                    break;
                }                
            }

            int slotRandomId = UnityEngine.Random.Range(0, emptySlots.Count);
            var slot = GetSlotById(emptySlots[slotRandomId]);
            emptySlots.Remove(slotRandomId);

            slot.SetItem(MergeData.itemsDictionary[(ItemTypes)typeIndex].items[0]);
        }
    }

    private List<int> FindEmptySlotsIndex()
    {
        List<int> emptySlots = new List<int>();
        int slotsCount = _slotSpawner.SlotDictionary.Count;

        for (int j = 0; j < slotsCount; j++)
        {
            bool isEmpty = _slotSpawner.SlotDictionary[j].CurrentItem == null;

            if (isEmpty) emptySlots.Add(j);
        }
        return emptySlots;
    }

    private Slot GetSlotById(int id)
    {
        return _slotSpawner.SlotDictionary[id];
    }

    private void InitChancesDictionary()
    {
        foreach(int chance in ItemTypeChances.Values)
        {
            _totalItemChances+= chance;
        }
    }
}