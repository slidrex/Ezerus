using UnityEngine;

namespace Ezerus
{
    public static class Functions
    {
        public static void EnableCursor(bool enable)
        {
            if(enable)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        public static Vector2 GetMousePosition() => Input.mousePosition;
    }
}
