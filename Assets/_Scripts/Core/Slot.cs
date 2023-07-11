using DG.Tweening;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, INotifyPropertyChanged
{
    [field: SerializeField] public ItemSO CurrentItem { get; protected set; }
    public event Action<Slot, bool> OnItemPressedChanged;
    public event Action<Slot, bool> OnItemOverlapChanged;
    public event Action<ItemSO> OnItemReceived;
    public event Action<ItemSO> OnItemMerged;
    public event PropertyChangedEventHandler PropertyChanged;

    protected CanvasGroup _canvasGroup;
    protected ItemPresenter _itemPresenter;
    [SerializeField] private Image underLayer;
    private ParticleSystem _vfx;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _itemPresenter = GetComponentInChildren<ItemPresenter>();
        if (_itemPresenter == null)
            Debug.LogError("Item presenter is null : ", gameObject);
        _vfx = GetComponentInChildren<ParticleSystem>();
        OnItemMerged += MergeFX;
        OnItemReceived += ChangeUnderLayer;
    }

    public void SetInteractable(bool flag)
    {
        _canvasGroup.blocksRaycasts = flag;
    }

    public virtual void SetItem(ItemSO item)
    {
        if (CurrentItem != null && item != null)
            OnItemMerged?.Invoke(item);

        CurrentItem = item;
        _itemPresenter.transform.position = transform.position;

        if (CurrentItem != null)
            _itemPresenter.SetItem(item);
        else
            _itemPresenter.Clear();

        OnItemSet(item);
    }

    public virtual bool TryPlace(Slot slot)
    {
        return CurrentItem == null;
    }

    public Transform GetItemTransform()
    {
        return _itemPresenter.transform;
    }

    protected void OnItemSet(ItemSO item)
    {
        OnItemReceived?.Invoke(item);
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected void ChangeUnderLayer(ItemSO item)
    {
        if (item == null)
            underLayer.color = Color.clear;
        else
        {
            underLayer.sprite = GameController.Game.Settings.ItemUnderLayers[item.Id];
            underLayer.color = Color.white;
        }
    }

    private void MergeFX(ItemSO newItem)
    {
        _vfx?.Play();
        DOTween.Sequence().Append(
            _itemPresenter.transform.DOScale(0, 0.2f)).Append(
            _itemPresenter.transform.DOScale(1, 0.4f));
    }

    public virtual void OnPointerDown(PointerEventData eventData) => OnItemPressedChanged?.Invoke(this, true);
    public virtual void OnPointerUp(PointerEventData eventData) => OnItemPressedChanged?.Invoke(this, false);
    public virtual void OnPointerEnter(PointerEventData eventData) => OnItemOverlapChanged?.Invoke(this, true);
    public virtual void OnPointerExit(PointerEventData eventData) => OnItemOverlapChanged?.Invoke(this, false);
}