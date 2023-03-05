using UnityEngine;
using UnityEngine.UI;

namespace Ezerus.UI
{
    public enum RenderUI
    {
        None,
        Inventory,
        TraderMenu,
    }
    public class UIRenderer : MonoBehaviour
    {
        [System.Serializable]
        private struct DefaultUI
        {
            public RenderUI Id;
            public RectTransform Transform;            
        }
        public Canvas Canvas;
        [SerializeField] private Image background;
        private System.Collections.Generic.Dictionary<RenderUI, RectTransform> attachedUI;
        private RenderUI usingUI;
        [SerializeField] private DefaultUI[] defaultUI;
        private RectTransform tempUI;
        private void Awake()
        {
            attachedUI = new System.Collections.Generic.Dictionary<RenderUI, RectTransform>();
            usingUI = RenderUI.None;
            for(int i = 0; i < defaultUI.Length; i++)
            {
                defaultUI[i].Transform.gameObject.SetActive(false);
                attachedUI.Add(defaultUI[i].Id, defaultUI[i].Transform);
            }
        }
        public void CacheUI(RectTransform UI, RenderUI id)
        {
            UI.transform.SetParent(transform);
            UI.transform.localScale = Vector3.one;
            UI.transform.localPosition = Vector3.zero;
            attachedUI.Add(id, UI);
        }
        public bool ContainsUI(RenderUI id) => attachedUI.ContainsKey(id);
        public RectTransform GetUI(RenderUI id) => attachedUI[id];

        public void AttachTempUI(RectTransform UI, bool defaultConfiguring = false)
        {
            if(tempUI != null) throw new System.Exception("UI is already attached!");
            if(defaultConfiguring) 
            {
                UI.transform.SetParent(transform);
                UI.transform.localScale = Vector3.one;
                UI.transform.localPosition = Vector3.zero;
            }
            UI.gameObject.SetActive(false);
            tempUI = UI;
        }
        public void DetachTempUI(bool destroy = true)
        {
            if(destroy)
                Destroy(tempUI);
            else tempUI.SetParent(null);
            tempUI = null;
            
        }
        public void EnableUIMode(RenderUI id)
        {
            gameObject.SetActive(true);
            background.gameObject.SetActive(true);
            Ezerus.Functions.EnableCursor(true);
            attachedUI[id].gameObject.SetActive(true);
            usingUI = id;
        }
        public void DisableUIMode()
        {
            background.gameObject.SetActive(false);
            attachedUI[usingUI].gameObject.SetActive(false);
            gameObject.SetActive(false);
            Ezerus.Functions.EnableCursor(false);
            usingUI = RenderUI.None;
        }
    }
}
