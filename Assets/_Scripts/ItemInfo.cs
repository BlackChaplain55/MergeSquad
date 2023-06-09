using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour 
{
    public int SlotId;
    public int ItemId;
    public MergeData.ItemTypes Type;
    public Image Visual;

    public void InitDummy(int slotId, int itemId, MergeData.ItemTypes type) 
    {
        this.SlotId = slotId;
        this.ItemId = itemId;
        this.Type = type;
        Visual.sprite = MergeData.GetItemVisualById(type, itemId);
    }
}