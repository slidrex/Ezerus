namespace Ezerus.Inventory
{
    public abstract class UsableItem : Item
    {
        public override Type ItemType => throw new System.NotImplementedException();
        protected abstract float Cooldown { get; set; }
        protected bool CooldownEnd {get; private set;}
        protected bool IsHandleCooldown;
        protected float timeSinceCooldown { get; private set; }
        public override ushort MaxStackCount { get; protected set; } = 1;
        public override void Update()
        {
            base.Update();
            if(IsHandleCooldown) HandleCooldown();
        }
        private void HandleCooldown()
        {
            timeSinceCooldown += UnityEngine.Time.deltaTime;
            if(timeSinceCooldown >= Cooldown) 
            {
                CooldownEnd = true;
                IsHandleCooldown = false;
            }
        }
        protected void SendUseRequest(bool ignoreCooldown = false) 
        {
            if((CooldownEnd || ignoreCooldown) && OnUseRequest()) OnUse();
        }
        protected virtual bool OnUseRequest()
        {
            return true;
        }
        protected virtual void OnUse() {}
        protected void ResetCooldown() 
        {
            timeSinceCooldown = 0.0f;
        }
        protected void StartCooldown()
        {
            IsHandleCooldown = true;
            CooldownEnd = false;
        }
    }
}
