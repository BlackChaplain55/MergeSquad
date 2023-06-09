using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int Id;
    public Item currentItem;
    public SlotState state = SlotState.Empty;

    public void CreateItem(int id, MergeData.ItemTypes type) 
    {
        var itemGO = (GameObject)Instantiate(Resources.Load("GamePrefabs/Item"));

        itemGO.transform.SetParent(this.transform);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localScale = Vector3.one;

        currentItem = itemGO.GetComponent<Item>();
        currentItem.Init(id, this, type);

        ChangeStateTo(SlotState.Full);
    }

    private void ChangeStateTo(SlotState targetState)
    {
        state = targetState;
    }

    public void ItemGrabbed()
    {
        Destroy(currentItem.gameObject);
        ChangeStateTo(SlotState.Empty);
    }
}

public enum SlotState
{
    Empty,
    Full
}