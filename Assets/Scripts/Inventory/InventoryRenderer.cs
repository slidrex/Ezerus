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
        private void Awake()
        {
            InitSlots();
        }
        private void InitSlots()
        {
            slots = new InventorySlot[hotBarHolder.childCount + slotHolder.childCount];
            int hotbarSlotCount = hotBarHolder.childCount;
            for(int i = 0; i < hotBarHolder.childCount; i++)
            {
                slots[i] = hotBarHolder.GetChild(i).GetComponent<InventorySlot>();
            }
            for(int i = 0; i < slotHolder.childCount; i++)
            {
                slots[hotbarSlotCount + i] = slotHolder.GetChild(i).GetComponent<InventorySlot>();
            }
        }
        private void OnEnable()
        {
            RenderSlots();
        }
        private void RenderSlots()
        {
            Inventory.InventoryItem[] items = attachedInventory.GetItemsClone();
            for(int i = 0; i < items.Length; i++)
            {
                if(items[i].Item == null) slots[i].SetDefaultRender();
                else slots[i].RenderSlot(items[i].Item.Sprite, items[i].CurrentStackCount);
            }
        }
    }
}
