using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MergeSystem : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private SlotSpawner slotSpawner;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera _camera;
    [SerializeField] private float coroutinesTimeStep;
    private Coroutine _carryRoutine;
    private int carryingDelta = 10;
    private Slot _carryingItemSlot;
    private Slot _targetItemSlot;
    private Transform _itemTransform;

    private void Start()
    {
        input.ClickEvents[InputActionPhase.Canceled] += () => OnItemDroped(_targetItemSlot);
    }

    public void OnItemSelected(Slot slot)
    {
        if (_carryRoutine != null)
            StopCoroutine(_carryRoutine);

        if (slot.CurrentItem == null) return;

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
        if (_targetItemSlot == null)
        {
            OnItemCarryFail(_carryingItemSlot);
            return;
        }

        if (_carryingItemSlot.CurrentItem == null) return;

        if (_carryingItemSlot == _targetItemSlot)
        {
            OnItemCarryFail(_carryingItemSlot);
            return;
        }

        if (_targetItemSlot.CurrentItem == null)
        {
            if (_targetItemSlot.TryPlace(_carryingItemSlot))
                PlaceInEmptySlot(_carryingItemSlot, _targetItemSlot);
            else
                OnItemCarryFail(_carryingItemSlot);

            return;
        }

        if (_targetItemSlot is EquipmentSlot)
        {
            if (_targetItemSlot.TryPlace(_carryingItemSlot))
                PlaceInEmptySlot(_carryingItemSlot, _targetItemSlot);
            else
                OnItemCarryFail(_carryingItemSlot);
            return;
        }

        if (TryMerge(_carryingItemSlot.CurrentItem, _targetItemSlot.CurrentItem))
            OnItemMergedWithTarget(_carryingItemSlot, _targetItemSlot);
        else
            OnItemCarryFail(_carryingItemSlot);
    }

    public void OnHover(Slot slot)
    {
        if (_carryingItemSlot == slot) return;

        slot.GetComponent<Image>().color = Color.green;

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
            Vector3 pointer = GetPointerPosition(true);
            itemTransform.position = new Vector3(pointer.x, pointer.y, itemTransform.position.z);

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
        emptySlot.SetItem(carryingItemSlot.CurrentItem);
        _carryingItemSlot.GetItemTransform().SetParent(_carryingItemSlot.transform, false);
        carryingItemSlot.SetItem(null);
        _carryingItemSlot.SetInteractable(true);
    }

    private void OnItemMergedWithTarget(Slot carryingItemSlot, Slot targetSlot)
    {
        int id = carryingItemSlot.CurrentItem.Id + 1;
        var type = carryingItemSlot.CurrentItem.Type;
        targetSlot.SetItem(MergeData.itemsDictionary[type].items[id]);
        _carryingItemSlot.GetItemTransform().SetParent(_carryingItemSlot.transform, false);
        carryingItemSlot.SetItem(null);
        _carryingItemSlot.SetInteractable(true);
    }

    private void OnItemCarryFail(Slot slot)
    {
        float animationLifeTime = 0.5f;
        slot.SetInteractable(false);
        var itemTransform = slot.GetItemTransform();

        itemTransform.DOMove(slot.transform.position, animationLifeTime);

        DOVirtual.DelayedCall(animationLifeTime,
            () =>
            {
                slot.GetItemTransform().SetParent(slot.transform);
                slot.SetInteractable(true);
            });
    }

    private Vector3 GetPointerPosition(bool isCamera)
    {
        float canvasMultiplier = 1 / (canvas.scaleFactor * 2);
        Vector3 pointerPosition = Input.mousePosition;
        if (isCamera)
            pointerPosition = _camera.ScreenToWorldPoint(pointerPosition);
        else
        {
            pointerPosition.x = pointerPosition.x - Screen.width * canvasMultiplier;
            pointerPosition.y = pointerPosition.y - Screen.height * canvasMultiplier;
        }
        pointerPosition.z = 0;
        return pointerPosition;
    }

    private void OnValidate()
    {
        if (input == null)
            input = Resources.Load<InputReader>("Input/GenericInputReader");
        if (_camera == null)
            _camera = Camera.main;
    }
}