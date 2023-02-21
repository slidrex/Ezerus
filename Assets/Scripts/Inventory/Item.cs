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
    }
}