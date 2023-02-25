using UnityEngine;
using Ezerus.Inventory;
public class CollectableItem : CollectableObject
{
    [Header("Other")]
    [SerializeField] private Item contentItem;
    private Inventory inventory;

    protected override void Start()
    {
        base.Start();
        inventory = FindObjectOfType<Inventory>();
    }

    protected override void OnCollect()
    {
        base.OnCollect();
        inventory.AddItem(contentItem);
        Destroy(gameObject);
    }
}
