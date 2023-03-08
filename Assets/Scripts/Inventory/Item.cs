using UnityEngine;

namespace Ezerus.Inventory
{
    public abstract class Item : ScriptableObject
    {
        public enum Rarity
        {
            Common,
            Rare,
            Mythic,
            Exotic   
        }
        public enum Type
        {
            Material,
            Sword,
            Staff,
            Ability,
            Money,
            Consumable
        }
        public string Name;
        public string Description;
        public Sprite Sprite;
        public Rarity Quality;
        public Inventory.StackItem StackItem;
        public abstract Type ItemType { get; }
        public abstract ushort MaxStackCount { get; protected set; }
        public virtual void Update() {}
        public bool IsStackable() => MaxStackCount > 1;
        public virtual void OnAttach(Entity entity) {}
        public virtual void OnDetach(Entity entity, Inventory.DetachType detachType) {}
        public int GetMaxStackCount() => MaxStackCount;
        public void PrimaryUse(Entity entity) => OnItemPrimaryUse(entity);
        public void SecondaryUse(Entity entity) => OnItemSecondaryUse(entity);
        public void SecondaryUseButtonUp(Entity entity) => OnItemSecondaryUseButtonUp(entity);
        public void Select(Entity entity) => OnItemSelect(entity);
        public void Deselect(Entity entity) => OnItemDeselect(entity);
        protected virtual void OnItemPrimaryUse(Entity entity) {}
        protected virtual void OnItemSecondaryUse(Entity entity) {}
        protected virtual void OnItemSecondaryUseButtonUp(Entity entity) {}
        protected virtual void OnItemSelect(Entity entity) {}
        protected virtual void OnItemDeselect(Entity entity) {}
    }
}