using UnityEngine;

namespace Ezerus.Inventory
{
    [RequireComponent(typeof(Inventory))]
    public class InventoryDropExtension : MonoBehaviour
    {
        [SerializeField] private KeyCode dropKey;
        private Inventory inventory;
        private void Awake()
        {
            inventory = GetComponent<Inventory>();
        }
        public void DropItem(int index)
        {
            inventory.RemoveAt(index, inventory.GetItem(index).StackCount, Inventory.DetachType.Drop);
        }
        public void DropItem(int index, int count)
        {
            inventory.RemoveAt(index, count, Inventory.DetachType.Drop);
        }
    }
}
