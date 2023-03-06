using UnityEngine;
using Ezerus.Inventory;

public class CollectableObject : MonoBehaviour
{
    [Header("Collect Effects")]
    [SerializeField] private GameObject collectEffect;
    [SerializeField] private float timeToDestroyEffect = 1;
    [SerializeField] private float amplitude = 1;
    [SerializeField] private float value = 0;
    protected virtual void Start()
    {
    }
    protected virtual void Update()
    {
        
        transform.Rotate(0, Mathf.Sin(1) * Time.deltaTime * amplitude, 0);
    }
    protected virtual void OnCollect(Inventory inventory)
    {
        if(collectEffect != null)
        {
            GameObject effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(effect, timeToDestroyEffect);
        }
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent<Inventory>(out Inventory inventory))
        {
            OnCollect(inventory);
            Destroy(gameObject);
        }
    }
    protected virtual void OnDestroy() {}
}
