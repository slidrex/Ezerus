using UnityEngine;

namespace Ezerus.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public enum DetachType
        {
            Drop,
            Destroyed
        }
        public Entity AttachedEntity;
        public delegate void InventoryUpdatedCallback(int index);
        public InventoryUpdatedCallback OnInventoryChanged; 
        private const int IndexNotFound = -1;
        [SerializeField] private StackItem[] Items;
        private void Start()
        {
            for(int i = 0; i < Items.Length; i++)
            {
                Items[i].Inventory = this;
                if(Items[i].Item != null) 
                {
                    Item item = Instantiate(Items[i].Item);
                    Items[i].InsertItem(item);
                }
                Items[i].UserInventoryPosition = i;
            }
        }
        private void Update()
        {
            UpdateInventoryItems();
        }
        private void UpdateInventoryItems()
        {
            foreach(StackItem item in Items)
            {
                item.Item?.Update();
            }
        }
        public StackItem[] GetItemsClone()
        {
            StackItem[] items = new StackItem[Items.Length];
            for(int i = 0; i < Items.Length; i++)
            {
                items[i] = new StackItem(this);
                if(Items[i]?.Item != null)
                {
                    items[i].InsertItem(Instantiate(Items[i].Item));
                    items[i].StackCount = Items[i].StackCount;
                }
            }
            return items;
        }
        public int GetItemsCount(Item item)
        {
            int count = 0;
            foreach(StackItem inventoryItem in Items)
            {
                if(inventoryItem.Item != null && InventoryRenderer.AreTheSame(inventoryItem.Item, item))
                {
                    count += inventoryItem.StackCount;
                }
            }
            return count;
        }
        public void RemoveAt(int index, int count, Inventory.DetachType detachType)
        {
            int stackCount = Items[index].StackCount;
            stackCount -= count;
            if(stackCount < 0) throw new System.Exception("Stack is less than zero!");
            else if(stackCount == 0) Items[index].InsertItem(null);
            
            Items[index].StackCount = stackCount;
            OnInventoryChanged.Invoke(index);
        }
        public void RemoveItems(Item item, int requiredCount, Inventory.DetachType detachType)
        {
            int index = 0;
            foreach(StackItem currentItem in Items)
            {
                if(currentItem.Item != null && InventoryRenderer.AreTheSame(item, currentItem.Item))
                {
                    if(currentItem.StackCount >= requiredCount)
                    {
                        currentItem.StackCount -= requiredCount;
                        return;
                    }
                    else
                    {
                        requiredCount -= currentItem.StackCount;
                        currentItem.StackCount = 0;
                        currentItem.InsertItem(null);
                    }
                    OnInventoryChanged?.Invoke(index);
                }
                index++;
            }
            if(requiredCount > 0) throw new System.Exception("Insufficient items!");
        }
        public void SetInventoryItem(int index, StackItem item)
        {
            Inventory.StackItem oldItem = Items[index];
            Items[index].InsertItem(item.Item);
            Items[index].StackCount = item.StackCount;
            if(oldItem != null && oldItem.Equals(item) == false) OnInventoryChanged.Invoke(index);
        }
        public bool AddItem(Inventory.StackItem item)
        {
            if(item.Item.IsStackable() == false)
            {
                int index = GetFreeSlotIndex();
                if(index == IndexNotFound) return false;
                Items[index].InsertItem(item.Item);
                OnInventoryChanged.Invoke(index);
                return true;
            }
            else
            {
                int unhandledItems = item.StackCount;
                int freeStackIndex = GetFreeItemstack(item.Item);
                int newSlotPosition = GetFreeSlotIndex();
                //Loop:
                //If there are free stacks fill them in otherwise fill new slot until the slot is full
                while(unhandledItems > 0 && (freeStackIndex != IndexNotFound || newSlotPosition != IndexNotFound))
                {
                    if(freeStackIndex != IndexNotFound)
                    {
                        if(Items[freeStackIndex].StackCount + unhandledItems <= item.Item.MaxStackCount)
                        {
                            Items[freeStackIndex].StackCount += unhandledItems;
                            unhandledItems = 0;
                        }
                        else 
                        {
                            int freeStackStorage = (item.Item.GetMaxStackCount() - Items[freeStackIndex].StackCount);
                            unhandledItems -= freeStackIndex;
                            Items[freeStackIndex].StackCount = item.Item.GetMaxStackCount();
                        }
                        OnInventoryChanged.Invoke(freeStackIndex);
                    }
                    else if(newSlotPosition != IndexNotFound)
                    {
                        item.InsertItem(item.Item);

                        if(unhandledItems <= item.Item.GetMaxStackCount())
                        {
                            Items[newSlotPosition].StackCount = unhandledItems;
                            unhandledItems = 0;
                        }
                        else
                        {
                            unhandledItems -= item.Item.GetMaxStackCount();
                            Items[newSlotPosition].StackCount = item.Item.GetMaxStackCount();
                        }
                        OnInventoryChanged.Invoke(newSlotPosition);
                    }
                    else throw new System.Exception("Unknown implementation!"); //To do drop items handling
                    freeStackIndex = GetFreeItemstack(item.Item);
                    newSlotPosition = GetFreeSlotIndex();
                }
                if(unhandledItems == 0) return true;
                return false;
            }
        }
        public StackItem GetItem(int index) => Items[index];
        private int GetFreeItemstack(Item item)
        {
            for(int i = 0; i < Items.Length; i++)
                if((Items[i].Item != null && InventoryRenderer.AreTheSame(Items[i].Item, item) && Items[i].StackCount < Items[i].Item.GetMaxStackCount()))
                    return i;
            
            return IndexNotFound;
        }
        private int GetFreeSlotIndex()
        {
            for(int i = 0; i < Items.Length; i++)
                if(Items[i].Item == null) return i;
            
            return IndexNotFound;
        }
        [System.Serializable]
        public class StackItem
        {
            internal Inventory Inventory { get; set; }
            [field: SerializeField] public Item Item { get; private set; }
            public int UserInventoryPosition { get; internal set; }
            internal StackItem(Inventory inventory) => Inventory = inventory;
            public void InsertItem(Item item, Inventory.DetachType detachType = Inventory.DetachType.Destroyed)
            {
                Item = item;
                if(item != null) 
                {
                    Item.StackItem = this;
                    item.OnAttach(Inventory.AttachedEntity);
                }
                else
                {
                    Item?.OnDetach(Inventory.AttachedEntity, detachType);
                    Item = item;
                }
            }
            public int StackCount = 1;
        }
    }
}
