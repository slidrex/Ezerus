using UnityEngine;
using Ezerus.Inventory;

namespace Ezerus.Inventory
{
    public class Ability : Item
    {
        public override Type ItemType => Type.Ability;
        protected Entity User { get; private set; }
        public override ushort MaxStackCount { get; protected set; } = 1;
        [SerializeField] protected int Cooldown;
        private float timeSinceCooldown;
        public bool ProcessCooldown { get; set; } = true;
        public float AbilityDuration;
        private float timeSinceAbilityUsed;
        private bool isUsed;
        protected override void OnItemSecondaryUse(Entity entity)
        {
            bool shouldUse = OnAbilityUseRequest();
            Debug.Log(name);
            if(entity.ContainsRule(IRuleHandler.Rule.BlockAbilities) == false && timeSinceCooldown >= (float)Cooldown && shouldUse) 
            {
                Debug.Log("start!");
                User = entity;
                OnAbilityUse();
            }
        }
        protected virtual bool OnAbilityUseRequest() => true;
        
        private void OnAbilityUse()
        {
            OnAbilityBegin();
            isUsed = true;
            timeSinceCooldown = 0.0f;
            timeSinceAbilityUsed = 0.0f;
        }
        public override void Update()
        {
            if(ProcessCooldown) HandleCooldown();
            if(isUsed) HandleAbilityDuration();
        }
        private void HandleCooldown()
        {
            if(timeSinceCooldown < Cooldown) timeSinceCooldown += Time.deltaTime;
            else 
            {
                ProcessCooldown = false;
            }
        }
        private void HandleAbilityDuration()
        {
            if(timeSinceAbilityUsed < AbilityDuration) 
            {
                timeSinceAbilityUsed += Time.deltaTime;
                OnAbilityLoop();
            }
            else
            {
                OnAbilityEnd();
                User = null;
                isUsed = false;
                System.Func<float> getter = () => timeSinceCooldown;
                int table;
                FindObjectOfType<CooldownTable>().CreateItem(getter, Cooldown);
                ProcessCooldown = true;
            }
        }
        protected virtual void OnAbilityBegin() {}
        protected virtual void OnAbilityLoop() {}
        protected virtual void OnAbilityEnd() {}
    }
}