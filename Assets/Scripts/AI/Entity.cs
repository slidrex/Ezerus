using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IRuleHandler
{
    [SerializeField] private int maxHealth;
    public int Health { get; private set; }
    public List<IRuleHandler.Rule> Rules { get; private set; }
    public Action<IRuleHandler.Rule> RuleChangeCallback { get; set; }
    protected virtual void Awake() => Rules = new List<IRuleHandler.Rule>();
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
    public void AddRule(IRuleHandler.Rule rule) 
    {
        Rules.Add(rule);
        RuleChangeCallback.Invoke(rule);
    }
    public void RemoveRule(IRuleHandler.Rule rule) 
    {
        Rules.Remove(rule);
        RuleChangeCallback.Invoke(rule);
    }
    public bool ContainsRule(IRuleHandler.Rule rule) => Rules.Contains(rule);
}
