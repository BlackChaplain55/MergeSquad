using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class ItemResources : ScriptableObject 
{
    public List<ItemSO> items;
    public string FullName;
    public string ShortName;
    public Sprite icon;


    //Resources.Load<LevelSO>("Item" + i);
}