using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Transform interactPoint;
    private void Awake()
    {
        if(interactPoint == null) interactPoint = transform;
    }
    public virtual void OnInteractEntityEnterZone(InteractManager interactEntity)
    {
        print("Interact entityh entered the zone");
    }
    public virtual void OnInteractEntityLeftZone(InteractManager interactEntity)
    {
        print("Interact entityh left the zone");
    }
    public virtual void OnInteractEntityInZone(InteractManager interactEntity)
    {

    }
    public virtual void OnInteractBeginToggle(InteractManager interactEntity)
    {
        
    }
    public virtual void OnInteractEndToggle(InteractManager interactEntity)
    {
        
    }
}
