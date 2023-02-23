using UnityEngine;

namespace Ezerus.Inventory
{
    public class InventoryHotbar : MonoBehaviour
    {
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private Inventory inventory;
        private InventoryHotbarSlot[] slots;
        public int SelectedSlot { get; private set; }
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
        }
        private void OnInventoryItemChanged(int index)
        {
            if(index < slots.Length) RenderSlot(index);
        }
        private void RenderSlot(int index)
        {
            InventoryHotbarSlot slot = slots[index];
            Inventory.InventoryItem item = inventory.GetItems()[index];
            if(item.Item == null) slot.SetDefaultRender();
            else
                slot.RenderSlot(item.Item.Sprite, item.CurrentStackCount);
        }
        private void RenderSlots()
        {
            for(int i = 0; i < slots.Length; i++) RenderSlot(i);
        }
    }

}
