using UnityEngine;

namespace Ezerus.Inventory
{
    public class StackableItem : Item
    {
        [field: SerializeField] public override ushort MaxStackCount { get; protected set; }
        public override Type ItemType => throw new System.NotImplementedException();
    }
}
