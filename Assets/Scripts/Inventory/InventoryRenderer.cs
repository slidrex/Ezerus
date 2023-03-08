using UnityEngine;
using UnityEngine.EventSystems;

namespace Ezerus.Inventory
{
    public class InventoryRenderer : MonoBehaviour
    {
        [SerializeField] private Transform slotHolder;
        [SerializeField] private Transform hotBarHolder;
        [SerializeField] private Inventory attachedInventory;
        private InventoryDropExtension dropExtension;
        private InventorySlot[] slots;
        private InventoryState state;
        private InventorySlot holdingSlotInstance;
        private Inventory.StackItem holdingItem;
        [SerializeField] private InventoryDescriptionPanel descriptionPanel; 
        [SerializeField] private Canvas canvas;
        private RectTransform canvasRect;
        private const float DescriptionPanelOffset = 200.0f;
        private enum InventoryState
        {
            None,
            Dragging
        }
        private void Awake()
        {
            dropExtension = attachedInventory.gameObject.GetComponent<InventoryDropExtension>();
            attachedInventory.OnInventoryChanged += RenderSlot;
            canvasRect = canvas.GetComponent<RectTransform>();
            InitSlots();
            descriptionPanel.ClearBody();
        }
        private void RenderSlot(int index)
        {
            RenderSlot(slots[index]);
        }
        private void Update()
        {
            if(state == InventoryState.Dragging) HoldDraggingSlot();
        }
        private void HoldDraggingSlot() => holdingSlotInstance.transform.position = Ezerus.Functions.GetMousePosition();
        public void OnSlotClicked(InventorySlot slot, PointerEventData pointerData)
        {
            Inventory.StackItem slotAssociatedItem = attachedInventory.GetItemsClone()[slot.SlotIndex];

            bool beginDraggingItem = state == InventoryState.None && slotAssociatedItem.Item != null;
            bool clickWithHoldingItem = state == InventoryState.Dragging;

            if(beginDraggingItem)
            {
                BeginDragItem(slot);
                bool unstackMode = pointerData.button == PointerEventData.InputButton.Right;

                if(unstackMode)
                {
                    UnstackSlotItems(slot, slotAssociatedItem);
                }
                else
                {
                    attachedInventory.SetInventoryItem(slot.SlotIndex, default(Inventory.StackItem));
                }
            }
            else if(clickWithHoldingItem)
            {
                bool droppedSlotContainsItem = slotAssociatedItem.Item != null;
                if(droppedSlotContainsItem)
                {
                    bool itemsStackable = slotAssociatedItem.Item.IsStackable() && holdingItem.Item.IsStackable();
                    
                    if(itemsStackable && AreTheSame(slotAssociatedItem.Item, holdingItem.Item) && slotAssociatedItem.StackCount < slotAssociatedItem.Item.MaxStackCount)
                    {
                        HandleStackableItems(slot.SlotIndex);
                    }
                    else
                        SwapHoldingItems(slot, slotAssociatedItem);
                }
                else
                {
                    EndDragItem();
                    attachedInventory.SetInventoryItem(slot.SlotIndex, holdingItem);
                }
            }
            if(state == InventoryState.Dragging && descriptionPanel.gameObject.activeSelf) descriptionPanel.gameObject.SetActive(false);
            else if(state == InventoryState.None && !descriptionPanel.gameObject.activeSelf && attachedInventory.GetItemsClone()[slot.SlotIndex].Item != null) SetupDescriptionPanel(slot);
            RenderSlot(slot);
        }
        private void UnstackSlotItems(InventorySlot slot, Inventory.StackItem slotAssociatedItem)
        {
            int holdingItemCount = Mathf.CeilToInt((float)slotAssociatedItem.StackCount / 2);
            int remainItemCount = slotAssociatedItem.StackCount - holdingItemCount;
            
            if(remainItemCount == 0) //If there is no remain items we remove item from slot
            {
                attachedInventory.SetInventoryItem(slot.SlotIndex, default(Inventory.StackItem));
            }
            else //Otherwise, separete slot items
            {
                holdingItem.StackCount = holdingItemCount;
                holdingSlotInstance.RenderSlot(holdingItem);
                Inventory.StackItem item = attachedInventory.GetItemsClone()[slot.SlotIndex];
                item.StackCount = remainItemCount;
                attachedInventory.SetInventoryItem(slot.SlotIndex, item);
            }
        }
        private void BeginDragItem(InventorySlot slot)
        {
            state = InventoryState.Dragging;
            holdingItem = attachedInventory.GetItemsClone()[slot.SlotIndex];
            holdingSlotInstance = Instantiate(slot, transform);
            holdingSlotInstance.ShowBackground(false);
        }
        private void SwapHoldingItems(InventorySlot slot, Inventory.StackItem slotItem)
        {
            attachedInventory.SetInventoryItem(slot.SlotIndex, holdingItem);
            holdingItem = slotItem;
            holdingSlotInstance.RenderSlot(slotItem.Item.Sprite, slotItem.StackCount);
        }
        private void HandleStackableItems(int dropOnSlot)
        {
            int extraItems = attachedInventory.GetItemsClone()[dropOnSlot].StackCount + holdingItem.StackCount - attachedInventory.GetItemsClone()[dropOnSlot].Item.MaxStackCount;
            if(extraItems <= 0) 
            {
                Inventory.StackItem item = attachedInventory.GetItemsClone()[dropOnSlot];
                item.StackCount += holdingItem.StackCount;
                attachedInventory.SetInventoryItem(dropOnSlot, item);
                EndDragItem();
            }
            else
            {
                attachedInventory.GetItemsClone()[dropOnSlot].StackCount = attachedInventory.GetItemsClone()[dropOnSlot].Item.MaxStackCount;
                holdingItem.StackCount = extraItems;
                holdingSlotInstance.RenderSlot(attachedInventory.GetItemsClone()[dropOnSlot].Item.Sprite, extraItems);
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
        private void OnSlotPointerEnter(InventorySlot slot, PointerEventData eventData)
        {
            if(attachedInventory.GetItemsClone()[slot.SlotIndex].Item != null) SetupDescriptionPanel(slot);
        }
        private void OnSlotPointerrExit(InventorySlot slot, PointerEventData eventData)
        {
            descriptionPanel.ClearBody();
            descriptionPanel.gameObject.SetActive(false);
        }
        private void SetupDescriptionPanel(InventorySlot slot)
        {
            descriptionPanel.ClearBody();
            Item item = attachedInventory.GetItemsClone()[slot.SlotIndex].Item;
            descriptionPanel.gameObject.SetActive(true);
            descriptionPanel.SetName(item.Name, item.Quality.GetRarityColor());
            descriptionPanel.PushBodyLine("Rarity: ", Color.white, item.Quality.ToString(), item.Quality.GetRarityColor());
            descriptionPanel.PushBodyLine("Type: ", Color.white, item.ItemType.ToString(), item.ItemType.GetTypeColor());
            descriptionPanel.PushBodyLine(item.Description, Color.grey);
            SetDescriptionPanelPosition(slot);
        }
        private void SetDescriptionPanelPosition(InventorySlot slot)
        {
            Vector3[] corners = new Vector3[4];
            canvasRect.GetWorldCorners(corners);
            Vector2 direction = Vector4.zero;
            foreach(Vector3 corner in corners)
            {
                Vector2 dist = slot.transform.position - corner;
                
                if(Mathf.Abs(dist.x) > Mathf.Abs(direction.x))  direction.x = dist.x;
                if(Mathf.Abs(dist.y) > Mathf.Abs(direction.y)) direction.y = dist.y; 
            }
            
            Vector2 panelOffset = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? -Vector2.right * Mathf.Sign(direction.x) : -Vector2.up * Mathf.Sign(direction.y);
            
            descriptionPanel.transform.position = (Vector3)slot.transform.position + (Vector3)panelOffset * DescriptionPanelOffset * Ezerus.Functions.GetAspectRatio();

        }
        private InventorySlot InitSlot(InventorySlot slot, int index)
        {
            slot.OnPointerClickCallback += OnSlotClicked;
            slot.OnPointerEnterCallback += OnSlotPointerEnter;
            slot.OnPointerExitCallback += OnSlotPointerrExit;
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
            Inventory.StackItem item = attachedInventory.GetItemsClone()[slot.SlotIndex];
            print(item);
            print(slot.SlotIndex);
            if(item.Item == null) slot.SetDefaultRender();
            else slot.RenderSlot(item.Item.Sprite, item.StackCount);
        }
        public static bool AreTheSame(Item first, Item second) => first.Name == second.Name;
    }
}
