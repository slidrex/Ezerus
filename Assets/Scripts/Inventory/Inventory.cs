using UnityEngine;

namespace Ezerus.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public delegate void InventoryUpdatedCallback(int index);
        public InventoryUpdatedCallback OnInventoryChanged; 
        private const int IndexNotFound = -1;
        [SerializeField] private InventoryItem[] items;
        public InventoryItem[] GetItemsClone() => items;
        public bool AddItem(Item item)
        {
            int changeRepositoryIndex = -1;
            if(item.GetMaxStackCount() <= 1)
            {
                int index = GetFreeSlotIndex();
                if(index == IndexNotFound) return false;
                items[index] = new InventoryItem { Item = item, CurrentStackCount = 1 }; 
                changeRepositoryIndex = index;
            }
            else
            {
                int index = GetFreeItemStack(item);
                if(index == IndexNotFound)
                {
                    index = GetFreeSlotIndex();
                    if(index == IndexNotFound) return false;
                    items[index] = new InventoryItem { Item = item, CurrentStackCount = 1 };
                }
                else
                    items[index].CurrentStackCount++;
                changeRepositoryIndex = index;
            }
            OnInventoryChanged.Invoke(changeRepositoryIndex);
            return true;
        }
        private int GetFreeItemStack(Item item)
        {
            for(int i = 0; i < items.Length; i++)
                if(items[i].Item.Equals(item) && items[i].CurrentStackCount < items[i].Item.GetMaxStackCount())
                    return i;
            
            return IndexNotFound;
        }
        private int GetFreeSlotIndex()
        {
            for(int i = 0; i < items.Length; i++)
                if(items[i].Item != null) return i;
            
            return IndexNotFound;
        }
        [System.Serializable]
        public struct InventoryItem
        {
            public Item Item;
            public int CurrentStackCount;
        }
    }
}
