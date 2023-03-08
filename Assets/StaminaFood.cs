using UnityEngine;
namespace Ezerus.Inventory
{
    [CreateAssetMenu(menuName = "Inventory Item/Consumable/Stamina Food")]
    public class StaminaFood : ConsumableItem 
    {
        public override ushort MaxStackCount { get; protected set; } = 8;
        private StaminaHolder holder;
        [SerializeField] private int restoreStamina;
        public override void OnAttach(Entity entity)
        {
            base.OnAttach(entity);
            holder = (entity as IStaminaHolder).StaminaHolder;
        }
        protected override bool ConsumeRequest()
        {
            return holder.CurrentStamina < holder.MaxStamina;
        }
        protected override void OnConsumeSuccess() => holder.AddStamina(restoreStamina);
    }
}