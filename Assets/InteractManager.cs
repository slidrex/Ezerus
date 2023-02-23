using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public Entity AttachedEntity;
    [SerializeField] private float interactRadius;
    protected System.Collections.Generic.List<InteractableObject> interactableObjectsInRadius = new System.Collections.Generic.List<InteractableObject>();
    private void Update()
    {
        UpdateInteractableObjects();
    }
    private void UpdateInteractableObjects()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, interactRadius, Vector3.one);
        
        foreach(RaycastHit hit in hits)
        {
            if((hit.transform.position - transform.position).sqrMagnitude > interactRadius * interactRadius) continue;
            InteractableObject entryObject = hit.collider.GetComponent<InteractableObject>();
            

            if(entryObject != null && interactableObjectsInRadius.Contains(entryObject) == false)
            {
            print(hit.collider.name);
                entryObject.OnInteractEntityEnterZone(this);
                interactableObjectsInRadius.Add(entryObject);
            }
        }
        for(int i = 0; i < interactableObjectsInRadius.Count; i++)
        {
            InteractableObject iterationObject = interactableObjectsInRadius[i];
            Vector3 distance = iterationObject.transform.position - transform.position;
            
            if(distance.sqrMagnitude > interactRadius * interactRadius)
            {
                iterationObject.OnInteractEntityLeftZone(this);
                interactableObjectsInRadius.RemoveAt(i);
            } else iterationObject.OnInteractEntityInZone(this);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
