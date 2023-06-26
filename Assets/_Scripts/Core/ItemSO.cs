using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ITEMS/Item")]
public class ItemSO : ScriptableObject
{
    public int Id;
    public Sprite ItemSprite;
    public ItemType Type;
}
