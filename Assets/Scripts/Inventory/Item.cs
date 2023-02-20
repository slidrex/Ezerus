using UnityEngine;

namespace Ezerus.Inventory
{
    public abstract class Item : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
        [SerializeField] private ushort MaxStackCount = 1;
        public int GetMaxStackCount() => MaxStackCount;
    }
}