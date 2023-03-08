using UnityEngine;
using Ezerus.Inventory;

public class CollectableItem : CollectableObject
{
    [Header("Item")]
    private const float spinningSpeed = 25.0f;
    private const float flowingSpeed = 2.5f;
    private const float flowingAmplitude = 0.4f;
    [SerializeField] private Inventory.StackItem contentItem;
    private Transform Transform;
    protected override void Start()
    {
        base.Start();
        Transform = transform;
    }
    protected override void OnCollect(Inventory inventory)
    {
        base.OnCollect(inventory);
        inventory.AddItem(contentItem);
        Destroy(gameObject);
    }
    protected override void Update()
    {
        base.Update();
        Vector3 position = Transform.position;
        Vector3 rotation = Transform.eulerAngles;
        if(rotation.y >= 360) rotation.y = 0.0f;
        Transform.Rotate(Vector3.up * spinningSpeed * Time.deltaTime);
        position.y += Mathf.Sin(Time.time * flowingSpeed) * Time.deltaTime * flowingAmplitude;
        Transform.position = position;
    }
}
