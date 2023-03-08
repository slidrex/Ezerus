using UnityEngine;

namespace Ezerus.Inventory
{
    public abstract class ConsumableItem : Item
    {
        public override Type ItemType => Type.Consumable;
        private EntityAttribute.PercentClaim consumeClaim;
        public virtual bool DeleteOnConsume { get; } = true;
        public override ushort MaxStackCount { get; protected set; }
        [SerializeField] protected float ConsumeSpeedModifier;
        [SerializeField] protected float ConsumeTime;
        protected virtual float ConsumeCooldown { get; } = 0.1f;
        private float timeSinceConsume;
        private bool handleCooldown;
        private float consumingTime;
        protected bool IsConsuming { get; set; }
        protected Entity Consumer;
        private Inventory attachedInventory;
        public override void OnAttach(Entity entity)
        {
            base.OnAttach(entity);
            if(ConsumeCooldown > 0.0f) handleCooldown = true;
            attachedInventory = entity.GetComponent<Inventory>();
            Consumer = entity;
        }
        public void InterruptConsuming(bool success)
        {
            consumingTime = 0.0f;
            consumeClaim.TryRemove();
            IsConsuming = false;
            Consumer.RemoveRule(IRuleHandler.Rule.BlockPlayerSprint);
            if(success) 
            {
                OnConsumeSuccess();
                if(DeleteOnConsume) DeleteItem();
            }
            else OnConsumeFail();
            OnConsumeEnd();
        }
        public override void Update()
        {
            base.Update();
            if(IsConsuming) HandleConsumeTime();
            if(handleCooldown) HandleCooldown();
        }
        private void DeleteItem()
        {
            attachedInventory.RemoveAt(StackItem.UserInventoryPosition, 1, Inventory.DetachType.Destroyed);
        }
        private void HandleCooldown()
        {
            timeSinceConsume += Time.deltaTime;
            if(timeSinceConsume >= ConsumeCooldown)
            {
                handleCooldown = false;
            }
        }
        private void HandleConsumeTime()
        {
            consumingTime += Time.deltaTime;
            if(consumingTime >= ConsumeTime)
            {
                InterruptConsuming(success: true);
                if(ConsumeCooldown > 0.0f)
                {
                    timeSinceConsume = 0.0f;
                    handleCooldown = true;
                }
            }
        }
        protected override void OnItemSecondaryUse(Entity entity)
        {
            if(IsConsuming == false && ConsumeRequest() && timeSinceConsume >= ConsumeCooldown) 
            {
                OnConsumeStarted();
                entity.AddRule(IRuleHandler.Rule.BlockPlayerSprint);
                IsConsuming = true;
                consumeClaim = entity.GetAttribute(EntityAttribute.Attribute.MovementSpeed).AddPercent(ConsumeSpeedModifier);
            }
        }
        protected override void OnItemSecondaryUseButtonUp(Entity entity)
        {
            if(IsConsuming) 
            {
                InterruptConsuming(false);
            }
        }
        protected virtual bool ConsumeRequest() { return true; }
        protected virtual void OnConsumeStarted() {}
        protected virtual void OnConsumeEnd() {}
        protected virtual void OnConsumeFail() {}
        protected virtual void OnConsumeSuccess() {}
    }
}
