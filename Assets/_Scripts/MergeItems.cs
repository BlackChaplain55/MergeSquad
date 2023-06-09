using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeItems : MonoBehaviour
{  
    [SerializeField] private Dictionary<MergeData.ItemTypes, int> itemTypeChances = new Dictionary<MergeData.ItemTypes, int>();  

    [Header("ItemChanses")]
    [SerializeField] private int _armourChance;
    [SerializeField] private int _SwordChance;
    [SerializeField] private int _bowChance;
    [SerializeField] private int _knifeChance;
    [SerializeField] private int _maceChance;
    [SerializeField] private int _staffChance;
    [SerializeField] private int _warriorAbilityChance;
    [SerializeField] private int _rangerAbilityChance;
    [SerializeField] private int _rogueAbilityChance;
    [SerializeField] private int _wizardAbilityChance;
    [SerializeField] private int _clericAbilityChance;

    private int totalItemChances = 0;
    private MergeConroller _mergeController;

    void Start()
    {

    }

    private void Awake()
    {
        InitChancesDictionary();
    }

    void Update()
    {
        _mergeController = GetComponent<MergeConroller>();
    }
    public void PlaceRandomItem()
    {
        for (int i = 1; i <= GameController.Game.Settings.ItemsPerRound; i++)
        {
            if (AllSlotsOccupied())
            {
                return;
            }
            int itemRandom = UnityEngine.Random.Range(0, totalItemChances);
            int currentChance = 0;
            int currentChanceAccum = 0;

            MergeData.ItemTypes currentType = MergeData.ItemTypes.Dummy;

            for (int itemIndex = 0; itemIndex < MergeConroller.TypesList.Count; itemIndex++)
            {
                currentType = MergeConroller.TypesList[itemIndex];
                currentChance = itemTypeChances.GetValueOrDefault(currentType);
                currentChanceAccum += currentChance;
                if (itemRandom <= currentChanceAccum)
                {
                    break;
                }                
            }

            int slotRandomId = UnityEngine.Random.Range(0, _mergeController.Slots.Count);
            Slot slot = GetSlotById(slotRandomId);
            while (slot.state == SlotState.Full)
            {
                slotRandomId = UnityEngine.Random.Range(0, _mergeController.Slots.Count);
                slot = GetSlotById(slotRandomId);
            }

            slot.CreateItem(0, currentType);
        }
    }

    bool AllSlotsOccupied()
    {
        foreach (var slot in _mergeController.Slots)
        {
            if (slot.state == SlotState.Empty)
            {
                return false;
            }
        }
        return true;
    }

    Slot GetSlotById(int id)
    {
        return _mergeController.slotDictionary[id];
    }

    private void InitChancesDictionary()
    {
        itemTypeChances.Add(MergeData.ItemTypes.Armour, _armourChance);
        itemTypeChances.Add(MergeData.ItemTypes.Sword, _SwordChance);
        itemTypeChances.Add(MergeData.ItemTypes.Bow, _bowChance);
        itemTypeChances.Add(MergeData.ItemTypes.Knife, _knifeChance);
        itemTypeChances.Add(MergeData.ItemTypes.Mace, _maceChance);
        itemTypeChances.Add(MergeData.ItemTypes.Staff, _staffChance);
        itemTypeChances.Add(MergeData.ItemTypes.WarriorAbility, _warriorAbilityChance);
        itemTypeChances.Add(MergeData.ItemTypes.RangerAbility, _rangerAbilityChance);
        itemTypeChances.Add(MergeData.ItemTypes.RogueAbility, _rogueAbilityChance);
        itemTypeChances.Add(MergeData.ItemTypes.WizardAbility, _wizardAbilityChance);
        itemTypeChances.Add(MergeData.ItemTypes.ClericAbility, _clericAbilityChance);

        foreach(int chance in itemTypeChances.Values)
        {
            totalItemChances+= chance;
        }
    }
}