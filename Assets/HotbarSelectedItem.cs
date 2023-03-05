using UnityEngine;
using UnityEngine.UI;

namespace Ezerus.Inventory
{
    public class HotbarSelectedItem : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private AnimationCurve fadeGraph;
        [SerializeField] private float duration;
        private float activationTime;
        public void Activate(string text)
        {
            gameObject.SetActive(true);
            _text.text = text;
            activationTime = 0.0f;
            _text.color = Color.white;
        }
        private void Update()
        {
            HandleAnimation();
        }
        private void HandleAnimation()
        {
            activationTime += Time.deltaTime / duration;
            Color color = _text.color;
            color.a = fadeGraph.Evaluate(activationTime);
            _text.color = color;

            if(activationTime >= 1.0f)
            {
                gameObject.SetActive(false);
            }

        }
    }

}
