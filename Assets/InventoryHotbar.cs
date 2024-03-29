using UnityEngine;

namespace Ezerus.Inventory
{
    public class InventoryHotbar : MonoBehaviour
    {
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private Inventory inventory;
        private InventoryHotbarSlot[] slots;
        public int SelectedSlot { get; private set; }
        [SerializeField] private KeyCode primaryUseKey;
        [SerializeField] private KeyCode secondaryUseKey;
        [SerializeField] private HotbarSelectedItem itemNameDisplayer;
        private void InitSlots()
        {
            slots = new InventoryHotbarSlot[slotsContainer.childCount];
            for(int i = 0; i < slotsContainer.childCount; i++)
            {
                slots[i] = slotsContainer.GetChild(i).GetComponent<InventoryHotbarSlot>();
            }
        }
        private void Awake()
        {
            InitSlots();
            inventory.OnInventoryChanged += OnInventoryItemChanged;
        }
        private void Start()
        {
            RenderSlots();
            SelectSlot(0);
        }
        private void Update()
        {
            if(Input.mouseScrollDelta != Vector2.zero)
            {
                SelectSlot((int)Mathf.Repeat(SelectedSlot - Mathf.Sign(Input.mouseScrollDelta.y), slots.Length));
            }
            SelectSlotsInput();
            SelectedItemUsageInput();
        }
        private void SelectedItemUsageInput()
        {
            if(Input.GetKey(primaryUseKey)) inventory.GetItem(SelectedSlot).Item?.PrimaryUse(inventory.AttachedEntity);
            else if(Input.GetKey(secondaryUseKey)) inventory.GetItem(SelectedSlot).Item?.SecondaryUse(inventory.AttachedEntity);
            else if(Input.GetKeyUp(secondaryUseKey)) inventory.GetItem(SelectedSlot).Item?.SecondaryUseButtonUp(inventory.AttachedEntity);
        }
        private void SelectSlotsInput()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
            else if(Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
            else if(Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
            else if(Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
            else if(Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        }
        private void SelectSlot(int slot)
        {
            Item newItem  = inventory.GetItemsClone()[slot].Item;
            inventory.GetItemsClone()[SelectedSlot].Item?.Deselect(inventory.AttachedEntity);
            slots[SelectedSlot].SetActiveBorder(false);
            if(newItem != null)
                itemNameDisplayer.Activate(newItem.Name);
            newItem?.Select(inventory.AttachedEntity);

            slots[slot].SetActiveBorder(true);
            SelectedSlot = slot;
        }
        private void OnInventoryItemChanged(int index)
        {
            if(index < slots.Length) RenderSlot(index);
        }
        private void RenderSlot(int index)
        {
            InventoryHotbarSlot slot = slots[index];
            Inventory.StackItem item = inventory.GetItemsClone()[index];
            print(index);
            if(item.Item == null) slot.SetDefaultRender();
            else
                slot.RenderSlot(item.Item.Sprite, item.StackCount);
        }
        private void RenderSlots()
        {
            for(int i = 0; i < slots.Length; i++) RenderSlot(i);
        }
    }

}
