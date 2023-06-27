﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public ItemSO CurrentItem { get; private set; }
    public event Action<Slot, bool> OnItemPressedChanged;
    public event Action<Slot, bool> OnItemOverlapChanged;
    private CanvasGroup _canvasGroup;
    private ItemPresenter _itemPresenter;
    public List<TextMeshProUGUI> textsRaycast;
    public List<Image> imgsRaycast;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _itemPresenter = GetComponentInChildren<ItemPresenter>();
        _itemPresenter.Clear();
    }

    public void SetInteractable(bool flag)
    {
        _canvasGroup.interactable = flag;
        _canvasGroup.blocksRaycasts = flag;
        foreach (var text in textsRaycast)
            text.raycastTarget = flag;
        foreach (var image in imgsRaycast)
            image.raycastTarget = flag;
    }

    public void SetItem(ItemSO item)
    {
        CurrentItem = item;
        if (CurrentItem != null)
            _itemPresenter.SetItem(item);
        else
        {
            _itemPresenter.transform.position = transform.position;
            _itemPresenter.Clear();
        }
    }

    public virtual bool TryPlace(Slot slot)
    {
        return CurrentItem == null;
    }

    public Transform GetItemTransform()
    {
        return _itemPresenter.transform;
    }

    public virtual void OnPointerDown(PointerEventData eventData) => OnItemPressedChanged?.Invoke(this, true);
    public virtual void OnPointerUp(PointerEventData eventData) => OnItemPressedChanged?.Invoke(this, false);
    public virtual void OnPointerEnter(PointerEventData eventData) => OnItemOverlapChanged?.Invoke(this, true);
    public virtual void OnPointerExit(PointerEventData eventData) => OnItemOverlapChanged?.Invoke(this, false);
}