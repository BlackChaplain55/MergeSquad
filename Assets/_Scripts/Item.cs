using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int Id;
    public MergeData.ItemTypes ItemType = MergeData.ItemTypes.Dummy;
    public Slot ParentSlot;
    public CharacterSlot CharParentSlot;
    public Image Icon;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _TypeText;

    public void Init(int id, Slot slot, MergeData.ItemTypes type)
    {
        this.Id = id;
        this.ParentSlot = slot;
        this.ItemType = type;
        this.Icon.sprite = MergeData.GetItemVisualById(type,id);
        this._levelText.text = id.ToString();
        this._TypeText.text = MergeData.GetShortName(type);
    }

    public void Init(int id, CharacterSlot slot, MergeData.ItemTypes type)
    {
        this.Id = id;
        this.CharParentSlot = slot;
        this.ItemType = type;
        this.Icon.sprite = MergeData.GetItemVisualById(type, id);
        this._levelText.text = id.ToString();
        this._TypeText.text = MergeData.GetShortName(type);
    }
}
