using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject collectEffect;
    [SerializeField] private float timeToDestroyEffect = 1;
    
    protected virtual void Start() {}
    protected virtual void Update() {}
    protected virtual void OnCollect()
    {
        if(collectEffect != null)
        {
            GameObject effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(effect, timeToDestroyEffect);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            OnCollect();
            Destroy(gameObject);
        }
    }
    protected virtual void OnDestroy()
    {

    }
}
