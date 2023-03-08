using UnityEngine;
using System.Collections.Generic;

public class EntityAttribute
{
    private System.Action<float> setter;
    private System.Func<float> getter;
    private float sourceValue;
    private List<PercentClaim> percentClaims;
    private float cachedValue;
    private bool attributeUpdated;
    public EntityAttribute(System.Action<float> set, System.Func<float> get, float sourceValue)
    {
        attributeUpdated = true;
        set.Invoke(sourceValue);
        getter = get;
        percentClaims = new List<PercentClaim>();
        setter = set;
        this.sourceValue = sourceValue;
    }
    public PercentClaim AddPercent(float percentValue)
    {
        attributeUpdated = true;
        PercentClaim claim = new PercentClaim(percentValue, this);
        percentClaims.Add(claim);
        UpdateProperty();
        return claim;
    }
    private void UpdateProperty() => setter.Invoke(GetValue());
    public float GetSource() => sourceValue;
    public float GetValue()
    {
        if(attributeUpdated == false) return cachedValue;
        else 
        {
            float src = sourceValue;
            float percentModifier = 1.0f;
            foreach(PercentClaim claim in percentClaims)
            {
                percentModifier += claim.claimValue;
            }
            src *= percentModifier;
            cachedValue = src;
            attributeUpdated = false;
            return src;
        }
    }
    public struct PercentClaim
    {
        private EntityAttribute attribute;
        public float claimValue { get; private set; }
        private bool deleted;
        public PercentClaim(float value, EntityAttribute attribute)
        {
            deleted = false;
            this.attribute = attribute;
            claimValue = value;
        }
        public bool NullOrDeleted() => deleted || attribute == null;
        public void Remove() 
        {
            attribute.percentClaims.Remove(this);
            attribute.attributeUpdated = true;
            attribute.UpdateProperty();
            deleted = true;
        }
        public bool TryRemove()
        {
            if(deleted == false && attribute != null)
            {
                Remove();
                return true;
            }
            return false;
        }
    }
    public enum Attribute
    {
        MovementSpeed,
        MaxHealth
    }
}
