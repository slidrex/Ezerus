using UnityEngine;
using UnityEngine.UI;

namespace Ezerus.Inventory
{
    public class InventoryRenderer : MonoBehaviour
    {
        [SerializeField] private Transform slotHolder;
        [SerializeField] private Transform hotBarHolder;
        [SerializeField] private Inventory attachedInventory;
        private InventorySlot[] slots;
        [SerializeField] private InventoryState state;
        private InventorySlot holdingSlotInstance;
        private Inventory.InventoryItem holdingItem;
        private enum InventoryState
        {
            None,
            Dragging
        }
        private void Awake()
        {
            InitSlots();
        }
        private void Update()
        {
            if(state == InventoryState.Dragging) HoldDraggingSlot();
        }
        private void HoldDraggingSlot() => holdingSlotInstance.transform.position = Ezerus.Functions.GetMousePosition();
        public void OnSlotClicked(InventorySlot slot)
        {
            Inventory.InventoryItem slotAssociatedItem = attachedInventory.Items[slot.SlotIndex];

            bool beginDraggingItem = state == InventoryState.None && slotAssociatedItem.Item != null;
            bool clickWithHoldingItem = state == InventoryState.Dragging;
            if(beginDraggingItem) 
            {
                BeginDragItem(slot);
            }
            else if(clickWithHoldingItem)
            {
                bool droppedSlotContainsItem = slotAssociatedItem.Item != null;
                if(droppedSlotContainsItem)
                {
                    bool itemsStackable = slotAssociatedItem.Item.IsStackable() && holdingItem.Item.IsStackable();
                    
                    if(itemsStackable && AreTheSame(slotAssociatedItem.Item, holdingItem.Item) && slotAssociatedItem.CurrentStackCount < slotAssociatedItem.Item.MaxStackCount)
                    {
                        HandleStackableItems(slot.SlotIndex);
                    }
                    else
                        SwapHoldingItems(slot, slotAssociatedItem);
                }
                else
                {
                    EndDragItem();
                    attachedInventory.Items[slot.SlotIndex] = holdingItem;
                }
            }
            RenderSlot(slot);
        }
        private void BeginDragItem(InventorySlot slot)
        {
            state = InventoryState.Dragging;
            holdingItem = attachedInventory.Items[slot.SlotIndex];
            holdingSlotInstance = Instantiate(slot, transform);
            attachedInventory.Items[slot.SlotIndex] = default(Inventory.InventoryItem);
            holdingSlotInstance.ShowBackground(false);
        }
        private void SwapHoldingItems(InventorySlot slot, Inventory.InventoryItem slotItem)
        {
            attachedInventory.Items[slot.SlotIndex] = holdingItem;
            holdingItem = slotItem;
            holdingSlotInstance.RenderSlot(slotItem.Item.Sprite, slotItem.CurrentStackCount);
        }
        private void HandleStackableItems(int dropOnSlot)
        {
            int extraItems = attachedInventory.Items[dropOnSlot].CurrentStackCount + holdingItem.CurrentStackCount - attachedInventory.Items[dropOnSlot].Item.MaxStackCount;
            if(extraItems <= 0) 
            {
                attachedInventory.Items[dropOnSlot].CurrentStackCount += holdingItem.CurrentStackCount;
                EndDragItem();
            }
            else
            {
                attachedInventory.Items[dropOnSlot].CurrentStackCount = attachedInventory.Items[dropOnSlot].Item.MaxStackCount;
                holdingItem.CurrentStackCount = extraItems;
                holdingSlotInstance.RenderSlot(attachedInventory.Items[dropOnSlot].Item.Sprite, extraItems);
            }
        }
        private void EndDragItem()
        {
            state = InventoryState.None;
            Destroy(holdingSlotInstance.gameObject);
        }
        private void InitSlots()
        {
            slots = new InventorySlot[hotBarHolder.childCount + slotHolder.childCount];
            int hotbarSlotCount = hotBarHolder.childCount;
            for(int i = 0; i < hotBarHolder.childCount; i++)
            {
                slots[i] = InitSlot(hotBarHolder.GetChild(i).GetComponent<InventorySlot>(), i);
            }
            for(int i = 0; i < slotHolder.childCount; i++)
            {
                slots[hotbarSlotCount + i] = InitSlot(slotHolder.GetChild(i).GetComponent<InventorySlot>(), hotbarSlotCount + i);
            }
        }
        private InventorySlot InitSlot(InventorySlot slot, int index)
        {
            slot.OnPointerClickCallback += OnSlotClicked;
            slot.InitSlot(index);
            return slot;
        }
        private void OnEnable()
        {
            RenderSlots();
        }
        private void RenderSlots()
        {
            for(int i = 0; i < slots.Length; i++)
            {
                RenderSlot(slots[i]);
            }
        }
        
        private void RenderSlot(InventorySlot slot)
        {
            Inventory.InventoryItem item = attachedInventory.Items[slot.SlotIndex];
            if(item.Item == null) slot.SetDefaultRender();
            else slot.RenderSlot(item.Item.Sprite, item.CurrentStackCount);
        }
        public static bool AreTheSame(Item first, Item second) => first.name == second.name;
    }
}
