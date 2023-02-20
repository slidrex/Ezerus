using UnityEngine;

namespace Ezerus
{
    public class PlayerMenuOpenHandler : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour player;
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
                menu.SetActive(isOpened);
            }
        }
        private void OnInventoryEnabled()
        {
            Functions.EnableCursor(true);
            player.BlockState = PlayerBehaviour.BlockingState.Blocked;
            
        }
        private void OnInventoryDisabled()
        {
            Functions.EnableCursor(false);
            player.BlockState = PlayerBehaviour.BlockingState.Free;
        }
    }
}
