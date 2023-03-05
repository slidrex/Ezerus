using UnityEngine;

namespace Ezerus.Trader
{
    public class TraderUI : MonoBehaviour
    {
        [SerializeField] private Transform selectedItemPosition;
        [SerializeField] private Transform itemsHolder;
        [SerializeField] private KeyCode nextItemKey;
        [SerializeField] private KeyCode previousItemKey;
        [SerializeField] private TraderUIDescriptionPanel panel;
        public RectTransform RectTransform { get; private set; }
        public int SelectedSlot { get; private set; }
        private System.Collections.Generic.List<TraderUIItem> m_items;
        [SerializeField] private TraderUIItem UIItem;
        [SerializeField] private Animation _animation;
        private Trader attachedTrader;
        private void Awake()
        {
            m_items = new System.Collections.Generic.List<TraderUIItem>();
            RectTransform = GetComponent<RectTransform>();
        }
        public void ReplaceItem(int index, Trader.TraderItem newItem)
        {
            m_items[index].RenderItem(newItem);
        }
        public void ConfigureItems(Trader trader, System.Collections.Generic.List<Trader.TraderItem> items)
        {
            attachedTrader = trader;
            int currentItemsLength = m_items.Count;
            int itemLength = items.Count;
            for(int i = 0; i < currentItemsLength; i++)
            {
                m_items[i].SlotIndex = i;
                m_items[i].RenderItem(items[i]);
            }
            if(currentItemsLength < itemLength)
            {
                int restItems = itemLength - currentItemsLength;
                for(int i = 0; i < restItems; i++)
                {
                    TraderUIItem item = Instantiate(UIItem);
                    item.transform.SetParent(itemsHolder);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = Vector3.one;
                    m_items.Add(item);
                    Trader.TraderItem traderItem = items[currentItemsLength + i];
                    item.SlotIndex = currentItemsLength + i;
                    item.RenderItem(traderItem);
                }
            }
            else if(currentItemsLength > itemLength)
            {
                for(int i = 0; i < m_items.Count; i++)
                {
                    Destroy(m_items[i].gameObject);
                    m_items.RemoveAt(i);
                }
            }
            ConfigureDescriptionTable();
        }
        private void ConfigureDescriptionTable()
        {
            panel.ConfigurePanel(attachedTrader.Items[SelectedSlot].SellItem.Item);
        }
        private void Start()
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(itemsHolder.GetComponent<RectTransform>());
            
            SelectSlot(0);
            itemsHolder.position += GetDistanceToSelectZone(GetSelectedSlotPosition());
        }
        private void Update()
        {
            if(Input.GetKeyDown(nextItemKey) && !_animation.InAnimation && SelectedSlot < m_items.Count - 1) SwitchItem(1);
            if(Input.GetKeyDown(previousItemKey) && !_animation.InAnimation && SelectedSlot > 0) SwitchItem(-1);
            if(_animation.InAnimation) _animation.UpdateAnimation();
            if(Input.GetKeyDown(KeyCode.Space) && _animation.InAnimation == false)
            {

            } 
        }
        public void TryBuyItem()
        {
            Ezerus.Trader.Trader.TraderItem priceItem = attachedTrader.Items[SelectedSlot];
            if(_animation.InAnimation == false)
            {
                if(attachedTrader.CanBuy(priceItem.PriceItem, priceItem.Price))
                {
                    //To do: buy
                    print("Item bought!");
                    attachedTrader.BuyItem(SelectedSlot);
                    Destroy(m_items[SelectedSlot].gameObject);
                    attachedTrader.Items.RemoveAt(SelectedSlot);

                    if(SelectedSlot > 0) SwitchItem(-1);
                    else if(SelectedSlot < m_items.Count - 1) SwitchItem(1);

                    m_items.RemoveAt(SelectedSlot);
                    
                }
                else 
                {
                    print("Not enough money!");
                    //To do: error: insiffucient items count
                }
            }
        }
        private void SwitchItem(int offset)
        {
            if(m_items[SelectedSlot] != null && m_items[SelectedSlot].Selected) m_items[SelectedSlot].PlaySelectAnimation();
            int targetSlot = SelectedSlot + offset;
            _animation.BeginAnimation(targetSlot, itemsHolder.position + GetDistanceToSelectZone(m_items[targetSlot].transform.position), itemsHolder);
            m_items[targetSlot].PlaySelectAnimation();
            _animation.AnimationEndCallback += OnAnimationEnd;
        }
        private void SelectSlot(int newSlot)
        {
            if(m_items[SelectedSlot].Selected) m_items[SelectedSlot].PlaySelectAnimation();
            m_items[newSlot].PlaySelectAnimation();
            SelectedSlot = newSlot;
        }
        private void OnAnimationEnd()
        {
            SelectedSlot = _animation.TargetSlot;
            ConfigureDescriptionTable();
        }

        private Vector3 GetSelectedSlotPosition() => m_items[SelectedSlot].transform.position;
        private Vector3 GetDistanceToSelectZone(Vector2 position) => selectedItemPosition.position - (Vector3)position;

        private struct Animation
        {
            internal bool InAnimation { get; private set; }
            internal int TargetSlot { get; private set; }
            private const float SelectingSpeed = 0.5f;
            internal Vector2 TargetPosition { get; private set; }
            internal System.Action AnimationEndCallback { get; set; }
            private Transform TransformItem {get; set; }
            private float time;
            private const float DestinationTime = 0.04f;
            internal void BeginAnimation(int targetItem, Vector2 targetPosition, Transform transformItem)
            {
                InAnimation = true;
                time = 0.0f;
                TargetPosition = targetPosition;
                TargetSlot = targetItem;
                TransformItem = transformItem;
            }
            internal void UpdateAnimation()
            {
                Vector2 currentPosition = TransformItem.position;
                TransformItem.position = Vector2.Lerp(currentPosition, TargetPosition, time);
                time += Time.deltaTime * SelectingSpeed;
                if(time >= DestinationTime)
                {
                    TransformItem.position = TargetPosition;
                    AnimationEndCallback.Invoke();
                    EndAnimation();
                }
            }
            internal void EndAnimation()
            {
                this = default(Animation);
            }
        } 
    }
}
