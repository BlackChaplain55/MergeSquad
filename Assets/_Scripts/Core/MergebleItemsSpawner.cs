using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class MergebleItemsSpawner : MonoBehaviour
{
    [field: SerializeField] public ItemTypesSpawnChance Chances { get; private set; }
    private int _totalItemChances = 0;
    private SlotSpawner _slotSpawner;

    private void OnValidate()
    {
        if (Chances == null)
            Chances = Resources.Load<ItemTypesSpawnChance>("GameSettings/SpawnChances");
    }

    private void Awake()
    {
        MergeData.InitResources();
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
            Array itemTypes = Enum.GetValues(typeof(ItemType));

            for (; typeIndex < itemTypes.Length; typeIndex++)
            {
                currentChance = Chances[(ItemType)typeIndex];
                currentChanceAccum += currentChance;
                if (itemRandom <= currentChanceAccum)
                {
                    break;
                }                
            }

            int slotRandomId = UnityEngine.Random.Range(0, emptySlots.Count);
            var slot = GetSlotById(emptySlots[slotRandomId]);
            emptySlots.Remove(slotRandomId);

            slot.SetItem(MergeData.itemsDictionary[(ItemType)typeIndex].items[0]);
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
}