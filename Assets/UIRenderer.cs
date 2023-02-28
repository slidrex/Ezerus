using UnityEngine;
using UnityEngine.UI;

namespace Ezerus.UI
{
    public class UIRenderer : MonoBehaviour
    {
        public Canvas Canvas;
        [SerializeField] private Image background;
        private System.Collections.Generic.List<RendererUI> attachedUI;
        private void Awake()
        {
            attachedUI = new System.Collections.Generic.List<RendererUI>();
        }
        public RendererUI AttachUI(RectTransform UI, bool defaultConfiguring = false)
        {
            RendererUI ui = new RendererUI()
            {
                UI = UI,
                OnDisableUIMode = null
            };
            if(defaultConfiguring) 
            {
                UI.transform.SetParent(transform);
                UI.transform.localScale = Vector3.one;
                UI.transform.localPosition = Vector3.zero;
            }
            attachedUI.Add(ui);
            return ui;
        }
        public void EnableUIMode(bool enable)
        {
            gameObject.SetActive(enable);
            background.gameObject.SetActive(enable);
            Ezerus.Functions.EnableCursor(enable);
            for(int i = 0; i < attachedUI.Count ; i++)
            {
                if(enable)
                    attachedUI[i].OnDisableUIMode?.Invoke();
                else attachedUI[i].OnEnableUIMode?.Invoke();
                if(attachedUI[i].UI == null) attachedUI.RemoveAt(i); 
            }
        }
        public struct RendererUI
        {
            public RectTransform UI;
            public System.Action OnEnableUIMode;
            public System.Action OnDisableUIMode;
        }
    }
}
