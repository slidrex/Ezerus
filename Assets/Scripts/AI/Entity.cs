using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IRuleHandler
{ 
    [SerializeField] private int maxHealth;
    public int Health { get; private set; }
    public List<IRuleHandler.Rule> Rules { get; private set; }
    public Action<IRuleHandler.Rule, IRuleHandler.ChangeType> RuleChangeCallback { get; set; }
    protected virtual void Awake() => Rules = new List<IRuleHandler.Rule>();
    private Dictionary<EntityAttribute.Attribute, EntityAttribute> attributes = new Dictionary<EntityAttribute.Attribute, EntityAttribute>();
    public void RegisterAttribute(System.Action<float> set, System.Func<float> get, float sourceValue, EntityAttribute.Attribute tag) => attributes.Add(tag, new EntityAttribute(set, get, sourceValue));
    public bool TryGetAttribute(EntityAttribute.Attribute tag, out EntityAttribute attribute) => attributes.TryGetValue(tag, out attribute);
    public EntityAttribute GetAttribute(EntityAttribute.Attribute tag) => attributes[tag];
    protected virtual void Start() 
    {
        RegisterAttributes();
    }
    protected virtual void Update() {}
    private void RegisterAttributes()
    {
        RegisterAttribute((float v) => maxHealth = (int)v, () => maxHealth, maxHealth, EntityAttribute.Attribute.MaxHealth);
    }
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
    public void AddRule(IRuleHandler.Rule rule) 
    {
        Rules.Add(rule);
        RuleChangeCallback.Invoke(rule, IRuleHandler.ChangeType.Add);
    }
    public void RemoveRule(IRuleHandler.Rule rule) 
    {
        Rules.Remove(rule);
        RuleChangeCallback.Invoke(rule, IRuleHandler.ChangeType.Remove);
    }
    public bool ContainsRule(IRuleHandler.Rule rule) => Rules.Contains(rule);
}
