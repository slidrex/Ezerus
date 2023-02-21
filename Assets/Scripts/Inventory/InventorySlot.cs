using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ezerus.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text index;
        [SerializeField] private Image background;
        [SerializeField] private Image itemImage;
        public int SlotIndex { get; private set; }
        public System.Action<InventorySlot> OnPointerClickCallback;
        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickCallback?.Invoke(this);
        }
        public void InitSlot(int index) => SlotIndex = index;
        public void RenderSlot(Sprite image, int count)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = image;
            index.text = count > 1 ? count.ToString() : "";
        }
        public void ShowBackground(bool show)
        {
            background.enabled = show;
        }
        public void SetDefaultRender()
        {
            itemImage.gameObject.SetActive(false);
            index.text = "";
        }
    }
}
