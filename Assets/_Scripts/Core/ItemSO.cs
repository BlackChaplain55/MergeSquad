using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ITEMS/Item")]
public class ItemSO : ScriptableObject, IItemStatsProvider, IComparable<ItemSO>
{
    public int Id;
    public float Power;
    public Sprite ItemSprite;
    public ItemType Type;

    public int CompareTo(ItemSO other)
    {
        if (Id > other.Id) return 1;
        if (Id < other.Id) return -1;
        return 0;
    }

    public float GetStats(ItemParameterType parameterType)
    {
        if (parameterType != ItemParameterType.Power) return -1;
        
        return Power;
    }
}
