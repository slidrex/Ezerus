using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int Health { get; private set; }
    protected virtual void Start() {}
    protected virtual void Update() {}
    public void Damage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            OnDie();
        }
        else
        {
            OnDamageTaken();
        }
    }
    public void Heal(int health)
    {
        Health = Mathf.Clamp(Health + health, 0, maxHealth);
        OnHealTaken();
    }
    protected virtual void OnDamageTaken() {}
    protected virtual void OnHealTaken() {}
    protected virtual void OnDie() {}
}
