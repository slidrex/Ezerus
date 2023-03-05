using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ezerus.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Text index;
        [SerializeField] private Image background;
        [SerializeField] private Image itemImage;
        public int SlotIndex { get; private set; }
        public System.Action<InventorySlot, PointerEventData> OnPointerClickCallback;
        public System.Action<InventorySlot, PointerEventData> OnPointerEnterCallback;
        public System.Action<InventorySlot, PointerEventData> OnPointerExitCallback;
        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickCallback?.Invoke(this, eventData);
        }
        public void InitSlot(int index) => SlotIndex = index;
        public void RenderSlot(Sprite image, int count)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = image;
            index.text = count > 1 ? count.ToString() : "";
        }
        public void RenderSlot(Inventory.StackItem item)
        {
            RenderSlot(item.Item.Sprite, item.StackCount);
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

        public void OnPointerEnter(PointerEventData eventData) => OnPointerEnterCallback?.Invoke(this, eventData);
        public void OnPointerExit(PointerEventData eventData) => OnPointerExitCallback?.Invoke(this, eventData);
    }
}
