using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public Entity AttachedEntity;
    [SerializeField] private float interactRadius;
    [SerializeField] private KeyCode interactionKey;
    public InteractableObject InteractObject { get; private set; }
    public bool InInteraction { get => InteractObject != null; }
    protected System.Collections.Generic.List<InteractableObject> interactableObjectsInRadius = new System.Collections.Generic.List<InteractableObject>();
    private void Update()
    {
        UpdateInteractableObjects();
        if((Input.GetKeyDown(interactionKey) || Input.GetKeyDown(KeyCode.Escape)) && InInteraction)
        {
            EndInteraction();
        }
        else if(Input.GetKeyDown(interactionKey) && interactableObjectsInRadius.Count > 0 && InInteraction == false && AttachedEntity.ContainsRule(IRuleHandler.Rule.BlockInteraction) == false)
        {
            AttachedEntity.AddRule(IRuleHandler.Rule.BlockInteraction);
            InteractObject = GetNearestInteractableObject();
            InteractObject.InteractBeginToggle(this);
        }
    }
    public void EndInteraction()
    {
        AttachedEntity.RemoveRule(IRuleHandler.Rule.BlockInteraction);
        InteractObject.InteractEndToggle(this);
        InteractObject = null;
    }
    private InteractableObject GetNearestInteractableObject()
    {
        float nearestSqrDistance = (interactableObjectsInRadius[0].transform.position - transform.position).sqrMagnitude;
        int targetIndex = 0;
        for(int i = 1; i < interactableObjectsInRadius.Count; i++)
        {
            float currentSqrDist = (interactableObjectsInRadius[i].transform.position - transform.position).sqrMagnitude;
            if(currentSqrDist <= nearestSqrDistance)
            {
                nearestSqrDistance = currentSqrDist;
                targetIndex = i;
            }
        }
        return interactableObjectsInRadius[targetIndex];
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
                entryObject.InteractEntityEnterZone(this);
                interactableObjectsInRadius.Add(entryObject);
            }
        }
        for(int i = 0; i < interactableObjectsInRadius.Count; i++)
        {
            InteractableObject iterationObject = interactableObjectsInRadius[i];
            Vector3 distance = iterationObject.transform.position - transform.position;
            
            if(distance.sqrMagnitude > interactRadius * interactRadius)
            {
                iterationObject.InteractEntityLeftZone(this);
                interactableObjectsInRadius.RemoveAt(i);
            } else iterationObject.InteractEntityInZone(this);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
