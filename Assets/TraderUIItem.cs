using UnityEngine;
using UnityEngine.UI;

namespace Ezerus.Trader
{
    public class TraderUIItem : MonoBehaviour
    {
        public bool Selected { get; private set; }
        [SerializeField] private AnimationCurve SelectScaleAnimationCurve;
        [SerializeField] private float targetAnimationTime;
        private bool inAnimation;
        private float animationTime;
        [Header("Holding item")]
        [SerializeField] private Text price;
        [SerializeField] private Image priceImage;
        [SerializeField] private Image itemImage;
        public int SlotIndex;
        public bool PlaySelectAnimation()
        {
            Selected = !Selected;
            inAnimation = true;
            return Selected;
        }
        public void RenderItem(Trader.TraderItem item)
        {
            this.price.text = item.Price.ToString();
            this.priceImage.sprite = item.PriceItem.Sprite;
            this.itemImage.sprite = item.SellItem.Item.Sprite;
        }
        private void Update()
        {
            if(inAnimation)
            {
                AnimationHandling();
            }
        }
        private void AnimationHandling()
        {
            animationTime += Selected ? Time.deltaTime : -Time.deltaTime;
            transform.localScale = SelectScaleAnimationCurve.Evaluate(animationTime/targetAnimationTime) * Vector2.one;
            if((Selected && animationTime >= targetAnimationTime) || (!Selected && animationTime <= 0)) 
            {
                inAnimation = false;
                animationTime = Mathf.Clamp(animationTime, 0.0f, targetAnimationTime);
            }
            
        }
    }
}
