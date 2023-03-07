using UnityEngine;

namespace Ezerus.Inventory
{
    public abstract class ConsumableItem : Item
    {
        public override Type ItemType => Type.Consumable;
        private EntityAttribute.PercentClaim consumeClaim;
        public virtual bool DeleteOnConsume { get; } = true;
        [field:SerializeField] public override ushort MaxStackCount { get; protected set; }
        [SerializeField] protected float ConsumeSpeedModifier;
        [SerializeField] protected float ConsumeTime;
        private float consumingTime;
        protected bool IsConsuming { get; set; }
        private Entity consumer;
        private Inventory attachedInventory;
        public override void OnAttach(Entity entity)
        {
            base.OnAttach(entity);
            attachedInventory = entity.GetComponent<Inventory>();
            consumer = entity;
        }
        public void InterruptConsuming(bool success)
        {
            consumingTime = 0.0f;
            consumeClaim.TryRemove();
            IsConsuming = false;
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
        }
        private void DeleteItem()
        {
            attachedInventory.RemoveAt(UserInventoryPosition, 1);
        }
        private void HandleConsumeTime()
        {
            consumingTime += Time.deltaTime;
            if(consumingTime >= ConsumeTime)
            {
                InterruptConsuming(success: true);
            }
        }
        protected override void OnItemSecondaryUse(Entity entity)
        {
            if(IsConsuming == false && ConsumeRequest()) 
            {
                OnConsumeStarted();
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
