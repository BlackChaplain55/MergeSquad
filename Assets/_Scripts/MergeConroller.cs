using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(MergeInput))]
[RequireComponent(typeof(MergeItems))]

public class MergeConroller : MonoBehaviour
{
    [SerializeField] private int _fieldSizeX;
    [SerializeField] private int _fieldSizeY;
    [SerializeField] private GameObject _slotTemplate;
    [SerializeField] private PartyPanelController _partyPanel;
    [SerializeField] private EnemyPanelController _enemyPanel;

    public static List<MergeData.ItemTypes> TypesList = new List<MergeData.ItemTypes>();
    public List<Slot> Slots = new List<Slot>();
    public Dictionary<int, Slot> slotDictionary;
    private GridLayoutGroup _grid;

    void Start()
    {
        if (_grid == null) _grid = GetComponent<GridLayoutGroup>();
        _grid.constraintCount = _fieldSizeX;
        slotDictionary = new Dictionary<int, Slot>();

        int slotsCount = _fieldSizeX * _fieldSizeY;

        for (int i = 0; i < slotsCount; i++) {
            GameObject newSlot = Instantiate(_slotTemplate, transform);
            Slots.Add(newSlot.GetComponent<Slot>());
        }

        for (int i = 0; i < Slots.Count; i++)
        {
            Slots[i].Id = i;
            slotDictionary.Add(i, Slots[i]);
        }
        FillItemsList();
        MergeData.InitResources();
    }

    public void Init()
    {
        _partyPanel.Init();
        _enemyPanel.Init();
    }

    private void FillItemsList()
    {
        TypesList.Add(MergeData.ItemTypes.Armour);
        TypesList.Add(MergeData.ItemTypes.Sword);
        TypesList.Add(MergeData.ItemTypes.Bow);
        TypesList.Add(MergeData.ItemTypes.Knife);
        TypesList.Add(MergeData.ItemTypes.Mace);
        TypesList.Add(MergeData.ItemTypes.Staff);
        TypesList.Add(MergeData.ItemTypes.WarriorAbility);
        TypesList.Add(MergeData.ItemTypes.RangerAbility);
        TypesList.Add(MergeData.ItemTypes.RogueAbility);
        TypesList.Add(MergeData.ItemTypes.WizardAbility);
        TypesList.Add(MergeData.ItemTypes.ClericAbility);
    }
}
