using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;

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
        EventBus.OnUnitDeath += unit => { if (unit.isEnemy) PlaceRandomItem(); };
        _slotSpawner = GetComponent<SlotSpawner>();

        foreach (var item in Chances.ItemTypeChances)
        {
            _totalItemChances += item.Value;
        }
    }

    [Button]
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
            int previousChanceAccum = 0;
            int typeIndex = 0;

            var chances = Chances.ItemTypeChances;
            int chancesCount = chances.Count;
            var keys = new List<ItemType>(chances.Keys);
            ItemType key = default;
            for (; typeIndex < chancesCount; typeIndex++)
            {
                key = keys[typeIndex];
                currentChance = chances[key];
                currentChanceAccum += currentChance;
                if (itemRandom <= currentChanceAccum)
                {
                    break;
                }                
            }

            int slotRandomId = UnityEngine.Random.Range(0, emptySlots.Count);
            var slot = GetSlotById(emptySlots[slotRandomId]);
            if (slot.CurrentItem != null) Debug.LogError("В слоте уже присутсвует предмет");
            emptySlots.RemoveAt(slotRandomId);
            var item = MergeData.itemsDictionary[key].GetItem(0);
            slot.SetItem(item);
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