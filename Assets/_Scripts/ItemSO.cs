using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ITEMS/Item")]
public class ItemSO : ScriptableObject
{
    public Sprite ItemSprite;
    public float power;
    public int Id;
    public MergeData.ItemTypes Type;
    public Slot ParentSlot;

    public void Init(int id, Slot slot)
    {
        this.Id = id;
        this.ParentSlot = slot; 
    }
}
