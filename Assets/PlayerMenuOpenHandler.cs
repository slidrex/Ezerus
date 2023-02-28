using UnityEngine;
using Ezerus.UI;

namespace Ezerus
{
    public class PlayerMenuOpenHandler : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private  UIRenderer Renderer;
        [SerializeField] private Ezerus.Inventory.InventoryRenderer inventory;
        [SerializeField] private KeyCode toggleKey;
        private bool isOpened;
        private void Update()
        {
            if(Input.GetKeyDown(toggleKey) && !(player.ContainsRule(IRuleHandler.Rule.BlockInteraction) && isOpened == false))
            {
                isOpened = !isOpened;
                EnableInventoryUI(isOpened);
                if(isOpened) OnInventoryEnabled();
                else OnInventoryDisabled();
            }
        }
        public void EnableInventoryUI(bool shouldEnable)
        {
            Renderer.EnableUIMode(shouldEnable);
        }
        private void OnInventoryEnabled()
        {
            inventory.gameObject.SetActive(true);
            player.AddRule(IRuleHandler.Rule.BlockCamera);
            player.AddRule(IRuleHandler.Rule.BlockInteraction);
            player.AddRule(IRuleHandler.Rule.BlockMovement);
        }
        private void OnInventoryDisabled()
        {
            inventory.gameObject.SetActive(false);
            player.RemoveRule(IRuleHandler.Rule.BlockCamera);
            player.RemoveRule(IRuleHandler.Rule.BlockInteraction);
            player.RemoveRule(IRuleHandler.Rule.BlockMovement);
        }
    }
}
