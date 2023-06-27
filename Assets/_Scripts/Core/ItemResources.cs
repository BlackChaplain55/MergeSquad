using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class ItemResources : ScriptableObject 
{
    public List<ItemSO> items;
    public string FullName;
    public string ShortName;
    public Sprite icon;

    public ItemSO GetItem(int index)
    {
        var item = items[index];
        if (item.Id != index)
        {
            items.Sort();
            return items[index];
        }
        return item;
    }
    //Resources.Load<LevelSO>("Item" + i);
}