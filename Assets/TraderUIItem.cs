using UnityEngine;
using UnityEngine.UI;

namespace Ezerus.UI
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
        public bool PlaySelectAnimation()
        {
            Selected = !Selected;
            inAnimation = true;
            return Selected;
        }
        public void RenderItem(uint price, Sprite priceImage, Sprite itemImage)
        {
            this.price.text = price.ToString();
            this.priceImage.sprite = priceImage;
            this.itemImage.sprite = itemImage;
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
