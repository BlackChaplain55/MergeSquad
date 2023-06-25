using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MergeSystem))]

public class SlotSpawner : MonoBehaviour
{
    [SerializeField] private int _fieldSizeX;
    [SerializeField] private int _fieldSizeY;
    [SerializeField] private MergeSystem mergeInput;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private GameObject slotTemplate;

    public Dictionary<int, Slot> SlotDictionary;
    private GridLayoutGroup _grid;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (mergeInput == null)
            mergeInput = GetComponent<MergeSystem>();
        if (slotsParent == null) return;
        if (_grid == null) _grid = slotsParent.GetComponent<GridLayoutGroup>();

        if (SlotDictionary != null) return;
        _grid.constraintCount = _fieldSizeX;
        SlotDictionary = new Dictionary<int, Slot>();

        int slotsCount = _fieldSizeX * _fieldSizeY;

        for (int i = 0; i < slotsCount; i++)
        {
            GameObject slotGO = Instantiate(slotTemplate, slotsParent);
            Slot slot = slotGO.GetComponent<Slot>();
            SlotDictionary.Add(i, slot);
            slot.OnItemPressedChanged += (slot, isPressed) =>
            {
                if (isPressed)
                    mergeInput.OnItemSelected(slot);
                else
                    mergeInput.OnItemDroped(slot);
            };
            slot.OnItemOverlapChanged += (slot, isBegin) =>
            {
                if (isBegin)
                    mergeInput.OnHover(slot);
                else
                    mergeInput.OnHoverEnd(slot);
            };
        }

        //DOVirtual.DelayedCall(0.1f, () => _grid.enabled = false);
    }
}
