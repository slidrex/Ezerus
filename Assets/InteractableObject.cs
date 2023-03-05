using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Transform interactPoint;
    protected InteractManager InteractEntity { get; private set; }
    private void Awake()
    {
        if(interactPoint == null) interactPoint = transform;
    }
    public void InteractEntityEnterZone(InteractManager interactEntity)
    {
        OnInteractEntityEnterZone();
    }
    public void InteractEntityLeftZone(InteractManager interactEntity)
    {
        OnInteractEntityLeftZone();
    }
    public void InteractEntityInZone(InteractManager interactEntity)
    {
        OnInteractEntityInZone();
    }
    public void InteractBeginToggle(InteractManager interactEntity)
    {
        InteractEntity = interactEntity;
        OnInteractEntityBeginToggle();
        
    }
    public void InteractEndToggle(InteractManager interactEntity)
    {
        OnInteractEntityEndToggle();
        InteractEntity = null;
    }
    protected virtual void OnInteractEntityEnterZone() {}
    protected virtual void OnInteractEntityLeftZone() {}
    protected virtual void OnInteractEntityInZone() {}
    protected virtual void OnInteractEntityBeginToggle() {}
    protected virtual void OnInteractEntityEndToggle() {}
}
