using UnityEngine;
using Ezerus.Inventory;

public class CollectableItem : CollectableObject
{
    [Header("Item")]
    [SerializeField] private Item contentItem;

    protected override void OnCollect(Inventory inventory)
    {
        base.OnCollect(inventory);
        inventory.AddItem(contentItem);
        Destroy(gameObject);
    }
}
