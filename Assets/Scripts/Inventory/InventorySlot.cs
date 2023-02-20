using UnityEngine;
using UnityEngine.UI;

namespace Ezerus.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Text index;
        [SerializeField] private Image background;
        [SerializeField] private Image itemImage;
        public void RenderSlot(Sprite image, int count)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = image;
            index.text = count.ToString();
        }
        public void SetDefaultRender()
        {
            itemImage.gameObject.SetActive(false);
            index.text = "";
        }
    }
}
