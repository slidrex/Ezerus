using UnityEngine;

public class CooldownTable : MonoBehaviour
{
    [SerializeField] private CooldownTableItem item;
    [SerializeField] private Transform elementsHolder;
    private System.Collections.Generic.List<CooldownTableItem> items = new System.Collections.Generic.List<CooldownTableItem>();
    public void CreateItem(System.Func<float> getter, float maxValue)
    {
        CooldownTableItem _item = Instantiate(item, elementsHolder);
        items.Add(_item);
        _item.Init(this, getter, maxValue);
    }
    public void OnItemCooldowned(CooldownTableItem item)
    {
        items.Remove(item);
        Destroy(item.gameObject);
    }
}