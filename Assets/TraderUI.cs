using UnityEngine;

namespace Ezerus.UI
{
    public class TraderUI : MonoBehaviour
    {
        [SerializeField] private Transform selectedItemPosition;
        [SerializeField] private Transform itemsHolder;
        [SerializeField] private KeyCode nextItemKey;
        [SerializeField] private KeyCode previousItemKey;
        public RectTransform RectTransform { get; private set; }
        public int SelectedSlot { get; private set; }
        private TraderUIItem[] items;
        [SerializeField] private Animation _animation;
        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            InitItems();
        }
        private void Start()
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(itemsHolder.GetComponent<RectTransform>());
            
            SelectSlot(0);
            itemsHolder.position += GetDistanceToSelectZone(GetSelectedSlotPosition());
        }
        private void Update()
        {
            if(Input.GetKeyDown(nextItemKey) && !_animation.InAnimation && SelectedSlot < items.Length - 1) SwitchItem(1);
            if(Input.GetKeyDown(previousItemKey) && !_animation.InAnimation && SelectedSlot > 0) SwitchItem(-1);
            if(_animation.InAnimation) _animation.UpdateAnimation();
        }
        private void SwitchItem(int offset)
        {
            if(items[SelectedSlot].Selected) items[SelectedSlot].PlaySelectAnimation();
            int targetSlot = SelectedSlot + offset;
            _animation.BeginAnimation(targetSlot, itemsHolder.position + GetDistanceToSelectZone(items[targetSlot].transform.position), itemsHolder);
            items[targetSlot].PlaySelectAnimation();
            _animation.AnimationEndCallback += OnAnimationEnd;
        }
        private void SelectSlot(int newSlot)
        {
            if(items[SelectedSlot].Selected) items[SelectedSlot].PlaySelectAnimation();
            items[newSlot].PlaySelectAnimation();
            SelectedSlot = newSlot;
        }
        private void OnAnimationEnd()
        {
            SelectedSlot = _animation.TargetSlot;
        }

        private Vector3 GetSelectedSlotPosition() => items[SelectedSlot].transform.position;
        private Vector3 GetDistanceToSelectZone(Vector2 position) => selectedItemPosition.position - (Vector3)position;
        private void InitItems()
        {
            int slotCount = itemsHolder.childCount;
            items = new TraderUIItem[slotCount];
            for(int i = 0; i < slotCount; i++)
            {
                items[i] = itemsHolder.GetChild(i).GetComponent<TraderUIItem>();
            }
        }
        [System.Serializable]
        private struct Animation
        {
            internal bool InAnimation { get; private set; }
            internal int TargetSlot { get; private set; }
            private const float SelectingSpeed = 0.5f;
            internal Vector2 TargetPosition { get; private set; }
            internal System.Action AnimationEndCallback { get; set; }
            private Transform TransformItem {get; set; }
            private float time;
            private const float DestinationTime = 0.1f;
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
