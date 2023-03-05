using UnityEngine;
using Ezerus.Inventory;

public class CollectableItem : CollectableObject
{
    [Header("Item")]
    [SerializeField] private Inventory.StackItem contentItem;

    protected override void OnCollect(Inventory inventory)
    {
        base.OnCollect(inventory);
        inventory.AddItem(contentItem);
        Destroy(gameObject);
    }
}
