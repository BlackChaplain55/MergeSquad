using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ITEMS/Item")]
public class ItemSO : ScriptableObject
{
    public Sprite ItemSprite;
    public float Power;
    public int Id;
    public ItemTypes Type;

    public void Init(int id)
    {
        this.Id = id; 
    }
}
