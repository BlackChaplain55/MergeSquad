using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MergeSystem : MonoBehaviour
{
    [SerializeField] private SlotSpawner slotSpawner;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float coroutinesTimeStep;
    private Coroutine _carryRoutine;
    private int carryingDelta=10;
    private Slot _carryingItemSlot;
    private Slot _targetItemSlot;
    private Transform _itemTransform;
    private Action OnClickCancelled;

    private void Start()
    {
        OnClickCancelled += () => OnItemDroped(_targetItemSlot);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            OnClickCancelled?.Invoke();
    }

    public void OnItemSelected(Slot slot)
    {
        if (_carryRoutine != null)
            StopCoroutine(_carryRoutine);

        slot.SetInteractable(false);
        _itemTransform = slot.GetItemTransform();
        _carryingItemSlot = slot;
        _itemTransform.SetParent(canvas.transform);
        _carryRoutine = StartCoroutine(CarryItem(coroutinesTimeStep, _itemTransform));
    }

    public void OnItemDroped(Slot slot)
    {
        if (_carryRoutine != null)
            StopCoroutine(_carryRoutine);

        if (_carryingItemSlot == null) return;
        if (_carryingItemSlot.CurrentItem == null) return;
        Debug.Log("_carryingItemSlot == slot : " + (_carryingItemSlot == slot));

        if (_carryingItemSlot == _targetItemSlot)
        {
            OnItemCarryFail(_carryingItemSlot);
            return;
        }
        Debug.Log("target : " + _targetItemSlot);
        if (_targetItemSlot.CurrentItem == null)
        {
            Debug.Log(2);
            if (_targetItemSlot.TryPlace(_carryingItemSlot))
                PlaceInEmptySlot(_carryingItemSlot, _targetItemSlot);
            else
                OnItemCarryFail(_carryingItemSlot);
        }

        if (TryMerge(_carryingItemSlot.CurrentItem, _targetItemSlot.CurrentItem))
            OnItemMergedWithTarget(_carryingItemSlot, _targetItemSlot);
        else
            OnItemCarryFail(_carryingItemSlot);
    }

    public void OnHover(Slot slot)
    {
        if (_carryingItemSlot == slot) return;

        slot.GetComponent<Image>().color = Color.red;

        _targetItemSlot = slot;
    }

    public void OnHoverEnd(Slot slot)
    {
        slot.GetComponent<Image>().color = Color.white;
        _targetItemSlot = null;
    }

    private IEnumerator CarryItem(float timeStep, Transform itemTransform)
    {
        WaitForSeconds wait = new(timeStep);

        while (true)
        {
            itemTransform.position = GetPointerPosition(true);

            yield return wait;
        }
    }

    private bool TryMerge(ItemSO carryingItem, ItemSO item2)
    {
        bool isNotMaxLevel = carryingItem.Id != GameController.Game.Settings.MaxItemLevel;
        bool isSameType = carryingItem.Type == item2.Type;
        bool isSameLevel = carryingItem.Id == item2.Id;
        return isNotMaxLevel && isSameType && isSameLevel;
    }

    private void PlaceInEmptySlot(Slot carryingItemSlot, Slot emptySlot)
    {
        Debug.Log("sdfdsf" + emptySlot, emptySlot);
        emptySlot.SetItem(carryingItemSlot.CurrentItem);
        _carryingItemSlot.GetItemTransform().SetParent(_carryingItemSlot.transform);
        carryingItemSlot.SetItem(null);
    }

    private void OnItemMergedWithTarget(Slot carryingItemSlot, Slot targetSlot)
    {
        int id = carryingItemSlot.CurrentItem.Id + 1;
        var type = carryingItemSlot.CurrentItem.Type;
        targetSlot.SetItem(MergeData.itemsDictionary[type].items[id]);
        _carryingItemSlot.GetItemTransform().SetParent(_carryingItemSlot.transform);
        carryingItemSlot.SetItem(null);
    }

    private void OnItemCarryFail(Slot slot)
    {
        slot.SetInteractable(false);
        var itemTransform = slot.GetItemTransform();
        DOTween.Sequence()
            .Append(itemTransform.DOMove(slot.transform.position, 0.5f))
            .onComplete = () =>
            {
                slot.GetItemTransform().SetParent(slot.transform);
                slot.SetInteractable(true);
            };
    }

    private Vector3 GetPointerPosition(bool isCamera)
    {
        float canvasMultiplier = 1 / (canvas.scaleFactor * 2);
        Vector3 pointerPosition = Input.mousePosition;
        if (isCamera)
            pointerPosition = Camera.main.ScreenToWorldPoint(pointerPosition);
        else
        {
            pointerPosition.x = pointerPosition.x - Screen.width * canvasMultiplier;
            pointerPosition.y = pointerPosition.y - Screen.height * canvasMultiplier;
        }
        pointerPosition.z = 0;
        return pointerPosition;
    }
}