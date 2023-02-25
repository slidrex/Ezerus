namespace Ezerus.Inventory
{
    public class NotStackableItem : Item
    {
        public override ushort MaxStackCount {get; protected set; } = 1;
        public override Type ItemType => throw new System.NotImplementedException();
    }
}
