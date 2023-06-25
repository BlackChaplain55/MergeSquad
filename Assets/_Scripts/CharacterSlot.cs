using UnityEngine;

public class CharacterSlot : Slot
{
    public CharController Char;
    public ItemTypes ItemType;

    public override bool TryPlace(Slot slot)
    {
        return ItemType == slot.CurrentItem.Type && CurrentItem.Id < slot.CurrentItem.Id;
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
