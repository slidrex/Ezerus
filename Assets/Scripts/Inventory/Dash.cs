using UnityEngine;

namespace Ezerus.Inventory
{
    [CreateAssetMenu(menuName = "Inventory Item/Ability/Dash")]
    public class Dash : Ability
    {
        public float DashSpeed;
        private Vector3 moveVector;
        private CharacterController userController;
        public override void OnAttach(Entity entity)
        {
            base.OnAttach(entity);
            userController = User.GetComponent<CharacterController>();
        }
        protected override void OnAbilityBegin()
        {
            User.AddRule(IRuleHandler.Rule.BlockMovement);
            moveVector = User.transform.forward;  
            
        }
        protected override void OnAbilityLoop()
        {
            userController.Move(moveVector * DashSpeed * Time.deltaTime);
        }
        protected override void OnAbilityEnd()
        {
            User.RemoveRule(IRuleHandler.Rule.BlockMovement);
        }
    }
}