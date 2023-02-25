using UnityEngine;

namespace Ezerus.UI
{
    public class TraderUIItem : MonoBehaviour
    {
        public bool Selected { get; private set; }
        public bool PlaySelectAnimation()
        {
            Selected = !Selected;
            return Selected;
        }
    }
}
