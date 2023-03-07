using UnityEngine;

namespace Ezerus.Inventory
{
    public abstract class Ability : UsableItem
    {
        public override Type ItemType => Type.Ability;
        protected Entity User { get; private set; }
        public override ushort MaxStackCount { get; protected set; } = 1;
        public float AbilityDuration;
        private float timeSinceAbilityUsed;
        [field: SerializeField] protected override float Cooldown {get; set;}
        private bool isUsed;
        private Ezerus.UI.CooldownTable cooldownTable;
        public override void OnAttach(Entity entity)
        {
            User = entity;
            cooldownTable = (User as Ezerus.UI.ICooldownTableHandler).Table;
            StartCooldown();
        }
        public override void OnDetach(Entity entity)
        {
            User = null;
        }
        protected override void OnItemSecondaryUse(Entity entity)
        {
            SendUseRequest();
        }
        protected override bool OnUseRequest() 
        {
            return User.ContainsRule(IRuleHandler.Rule.BlockAbilities) == false && isUsed == false;
        }
        
        protected override void OnUse()
        {
            OnAbilityBegin();
            isUsed = true;
            IsHandleCooldown = false;
            ResetCooldown();
            timeSinceAbilityUsed = 0.0f;
        }
        public override void Update()
        {
            base.Update();
            if(isUsed) HandleAbilityDuration();
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
                StartCooldown();
                isUsed = false;
                if(cooldownTable != null)
                {
                    System.Func<float> getter = () => timeSinceCooldown;
                    cooldownTable.CreateItem(getter, Cooldown);
                }
            }
        }
        protected virtual void OnAbilityBegin() {}
        protected virtual void OnAbilityLoop() {}
        protected virtual void OnAbilityEnd() {}
    }
}