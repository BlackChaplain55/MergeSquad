using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : Slot
{
    public float DeathTimer
    {
        get { return _deathTimer; }
        set
        {
            _deathTimer = value;
            OnPropertyChanged();
            ChangeBarValue();
        }
    }
    public ItemType ItemType;
    [SerializeField] private Image highlight;
    [SerializeField] protected Image bar;
    [SerializeField] protected Image barBackground;
    private float _deathTimer;
    private Sprite _placeHolder;
    private Color _placeHolderColor;
    private Color _highlightColor;
    private Tween highlightTween;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _placeHolder = GameController.Game.Settings.ItemsPlaceholders[ItemType];
        _placeHolderColor = _itemPresenter.GetColor();
        _itemPresenter.SetIcon(_placeHolder);

        if (highlight != null)
            _highlightColor = highlight.color;
    }

    public override void SetItem(ItemSO item)
    {
        CurrentItem = item;
        _itemPresenter.transform.position = transform.position;

        bool isItemExist = CurrentItem != null;
        if (isItemExist)
        {
            _itemPresenter.SetItem(item);
            _itemPresenter.SetColor(Color.white);
        }
        else
        {
            _itemPresenter.SetIcon(_placeHolder);
            _itemPresenter.SetColor(_placeHolderColor);
        }

        OnItemSet(item);

        if (bar == null) return;

        bar.enabled = isItemExist;
        barBackground.enabled = isItemExist;
    }

    public void ResetDeathTimer(IItemStatsProvider itemStats)
    {
        DeathTimer = itemStats.GetStats(ItemParameterType.DeathTimer);
    }

    public override bool TryPlace(Slot slot)
    {
        bool isSameType = ItemType == slot.CurrentItem.Type;
        bool isLowerLevel = true;
        if (CurrentItem != null)
            isLowerLevel = slot.CurrentItem.Id >= CurrentItem.Id;

        return isSameType && isLowerLevel;
    }

    public void SetHighlight(bool flag)
    {
        if (highlight == null)
            return;

        highlightTween?.Kill();
        Color color = flag? _highlightColor : Color.clear;

        highlight.enabled = true;
        highlightTween = highlight.DOColor(color, 0.2f);
        highlightTween.OnComplete(() => highlight.enabled = flag);
    }

    protected virtual void ChangeBarValue()
    {
        if (bar == null) return;

        Color hundred = Color.HSVToRGB(1 / 3f, 0.7f, 1);
        Color zero = Color.HSVToRGB(0, 0.7f, 1);
        var eqData = (EquipmentSO)CurrentItem;
        float value = DeathTimer / eqData.DeathTimer;
        Color color = Color.Lerp(zero, hundred, value);
        bar.fillAmount = value;
        bar.color = color;
        _itemPresenter.SetColor(color);
    }
}
