using UnityEngine;

namespace Ezerus
{
    public class PlayerMenuOpenHandler : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject menu;
        [SerializeField] private KeyCode toggleKey;
        private bool isOpened;
        private void Update()
        {
            if(Input.GetKeyDown(toggleKey))
            {
                isOpened = !isOpened;
                if(isOpened) OnInventoryEnabled();
                else OnInventoryDisabled();
                EnableInventoryUI(isOpened);
            }
        }
        public void EnableInventoryUI(bool shouldEnable)
        {
            menu.SetActive(isOpened);
        }
        private void OnInventoryEnabled()
        {
            Functions.EnableCursor(true);
            player.BlockState = Player.BlockingState.Blocked;
            
        }
        private void OnInventoryDisabled()
        {
            Functions.EnableCursor(false);
            player.BlockState = Player.BlockingState.Free;
        }
    }
}
