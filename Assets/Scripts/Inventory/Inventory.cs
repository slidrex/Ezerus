using UnityEngine;

namespace Ezerus.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public delegate void InventoryUpdatedCallback(int index);
        public InventoryUpdatedCallback OnInventoryChanged; 
        private const int IndexNotFound = -1;
        [SerializeField] public InventoryItem[] Items;
        private void Start()
        {
            for(int i = 0; i < Items.Length; i++)
            {
                if(Items[i].Item != null) Items[i].Item = Instantiate(Items[i].Item);
            }
        }
        public bool AddItem(Item item)
        {
            int changeRepositoryIndex = -1;
            if(item.IsStackable() == false)
            {
                int index = GetFreeSlotIndex();
                if(index == IndexNotFound) return false;
                Items[index] = new InventoryItem { Item = item, CurrentStackCount = 1 }; 
                changeRepositoryIndex = index;
            }
            else
            {
                int index = GetFreeItemstack(item);
                if(index == IndexNotFound)
                {
                    index = GetFreeSlotIndex();
                    if(index == IndexNotFound) return false;
                    Items[index] = new InventoryItem { Item = item, CurrentStackCount = 1 };
                }
                else
                    Items[index].CurrentStackCount++;
                changeRepositoryIndex = index;
            }
            OnInventoryChanged.Invoke(changeRepositoryIndex);
            return true;
        }
        private int GetFreeItemstack(Item item)
        {
            for(int i = 0; i < Items.Length; i++)
                if(InventoryRenderer.AreTheSame(Items[i].Item, item) && Items[i].CurrentStackCount < Items[i].Item.GetMaxStackCount())
                    return i;
            
            return IndexNotFound;
        }
        private int GetFreeSlotIndex()
        {
            for(int i = 0; i < Items.Length; i++)
                if(Items[i].Item != null) return i;
            
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
