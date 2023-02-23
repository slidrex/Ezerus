using UnityEngine;

public class InventoryHotbarSlot : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text itemsCount;
    [SerializeField] private UnityEngine.UI.Image itemImage;
    public void RenderSlot(Sprite image, int count)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = image;
        
        itemsCount.text = count > 1 ? count.ToString() : "";
    }
    public void SetDefaultRender()
    {
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
        itemsCount.text = "";
    }
}
