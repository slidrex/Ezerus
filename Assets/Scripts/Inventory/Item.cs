using UnityEngine;

namespace Ezerus.Inventory
{
    public abstract class Item : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
        public abstract ushort MaxStackCount { get; protected set; }
        public bool IsStackable() => MaxStackCount > 1;
        public int GetMaxStackCount() => MaxStackCount;
        public void PrimaryUse(Entity entity) => OnItemPrimaryUse(entity);
        public void SecondaryUse(Entity entity) => OnItemSecondaryUse(entity);
        public void Select(Entity entity) => OnItemSelect(entity);
        public void Deselect(Entity entity) => OnItemDeselect(entity);
        protected virtual void OnItemPrimaryUse(Entity entity) {}
        protected virtual void OnItemSecondaryUse(Entity entity) {}
        protected virtual void OnItemSelect(Entity entity) {}
        protected virtual void OnItemDeselect(Entity entity) {}
    }
}