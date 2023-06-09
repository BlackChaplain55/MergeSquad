using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    public int Id;
    public CharController Char;
    public Item currentItem;
    public MergeData.ItemTypes[] ItemTypes;
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

    //private void ReceiveItem(int id, MergeData.ItemTypes type)
    //{
    //    switch (state)
    //    {
    //        case SlotState.Empty: 

    //            CreateItem(id, type);
    //            ChangeStateTo(SlotState.Full);
    //            break;

    //        case SlotState.Full: 
    //            if (currentItem.Id == id)
    //            {
    //                //Merged
    //                Debug.Log("Merged");
    //            }
    //            else
    //            {
    //                Debug.Log("Push back");
    //            }
    //            break;
    //    }
    //}
}
