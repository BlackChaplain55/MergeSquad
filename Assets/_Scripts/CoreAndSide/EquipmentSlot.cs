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
    [SerializeField] private Image bar;
    [SerializeField] private Image barBackground;
    private float _deathTimer;
    private Sprite _placeHolder;
    private Color _placeHolderColor;

    protected override void Awake()
    {
        base.Awake();
        _placeHolder = _itemPresenter.GetSprite();
        _placeHolderColor = _itemPresenter.GetColor();
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
        bar.enabled = isItemExist;
        barBackground.enabled = isItemExist;
        OnItemSet(item);
    }

    public void ResetDeathTimer(EquipmentSO newItem)
    {
        DeathTimer = newItem.DeathTimer;
    }

    public override bool TryPlace(Slot slot)
    {
        bool isSameType = ItemType == slot.CurrentItem.Type;
        bool isLowerLevel = true;
        if (CurrentItem != null)
            isLowerLevel = slot.CurrentItem.Id >= CurrentItem.Id;

        return isSameType && isLowerLevel;
    }

    protected virtual void ChangeBarValue()
    {
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
