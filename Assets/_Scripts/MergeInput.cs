using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.EventSystem.RaycastAll;

public class MergeInput : MonoBehaviour
{
    private Vector3 _target;
    private int carryingDelta=10;
    private float grabbed;
    private ItemInfo carryingItem;
    private MergeConroller _mergeController;
    private Transform _canvasTransform;
    private EventSystem _eventSystem;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SendRayCast();
        }

        if (Input.GetMouseButton(0) && carryingItem)
        {
            OnItemSelected();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SendRayCast();
        }
    }

    private void Awake()
    {
        _mergeController = GetComponent<MergeConroller>();
        _canvasTransform = transform.parent;
        MergeData.InitResources();
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    void SendRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        Slot slot = new Slot();
        CharacterSlot charSlot = new CharacterSlot();

        var pointerEventData = new PointerEventData(_eventSystem) { position = Input.mousePosition };
        var raycastResults = new List<RaycastResult>();
        _eventSystem.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if(result.gameObject.TryGetComponent<Slot>(out slot))
                {
                    break;
                }else if (result.gameObject.TryGetComponent<CharacterSlot>(out charSlot))
                {
                    break;
                }
            }
        }
        if (slot != null)
        {
            if (slot.state == SlotState.Full && carryingItem == null)
            {
                CreateDummy(slot);
            }
            else if (slot.state == SlotState.Empty && carryingItem != null)
            {
                slot.CreateItem(carryingItem.ItemId, carryingItem.Type);
                Destroy(carryingItem.gameObject);
            }
            else if (slot.state == SlotState.Full && carryingItem != null)
            {
                if (slot.currentItem.Id == carryingItem.ItemId&& slot.currentItem.ItemType == carryingItem.Type)
                {
                    print("merged");
                    OnItemMergedWithTarget(slot.Id,carryingItem.Type);
                }
                else
                {
                    OnItemCarryFail();
                }
            }
        }else if (charSlot!=null)
        {
            if (carryingItem != null)
            {
                foreach(MergeData.ItemTypes type in charSlot.ItemTypes)
                {
                    if (carryingItem.Type == type)
                    {
                        charSlot.CreateItem(carryingItem.ItemId, carryingItem.Type);
                        Destroy(carryingItem.gameObject);
                    }
                }
                
            }
        }
        else
        {
            if (!carryingItem)
            {
                return;
            }
            OnItemCarryFail();
        }
    }

    private void CreateDummy(Slot slot)
    {
        var itemGO = (GameObject)Instantiate(Resources.Load("GamePrefabs/ItemDummy"), _canvasTransform);
        itemGO.transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y, 0);
        itemGO.transform.localScale = Vector3.one * 1.5f;

        carryingItem = itemGO.GetComponent<ItemInfo>();
        carryingItem.InitDummy(slot.Id, slot.currentItem.Id, slot.currentItem.ItemType);

        slot.ItemGrabbed();
    }

    void OnItemSelected()
    {
        _target = getPointerPosition();
        float delta = carryingDelta * Time.deltaTime;

        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.localPosition = Vector3.MoveTowards(carryingItem.transform.localPosition, _target, delta);
        Debug.Log("CarryinItem to "+ _target.x+":"+ _target.y);
    }

    private Vector3 getPointerPosition()
    {
        Vector3 pointerPosition = Input.mousePosition;
        pointerPosition.x = pointerPosition.x - Screen.width / _canvasTransform.GetComponent<Canvas>().scaleFactor / 2;
        pointerPosition.y = pointerPosition.y - Screen.height / _canvasTransform.GetComponent<Canvas>().scaleFactor / 2;
        pointerPosition.z = 0;
        return pointerPosition;
    }

    void OnItemMergedWithTarget(int targetSlotId, MergeData.ItemTypes type)
    {
        if (carryingItem.ItemId == GameController.Game.Settings.MaxItemLevel)
        {
            OnItemCarryFail();
            return;
        }
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.currentItem.gameObject);

        slot.CreateItem(carryingItem.ItemId + 1,type);

        Destroy(carryingItem.gameObject);
    }

    void OnItemCarryFail()
    {
        var slot = GetSlotById(carryingItem.SlotId);
        slot.CreateItem(carryingItem.ItemId, carryingItem.Type);
        Destroy(carryingItem.gameObject);
    }

    Slot GetSlotById(int id)
    {
        return _mergeController.slotDictionary[id];
    }
}
