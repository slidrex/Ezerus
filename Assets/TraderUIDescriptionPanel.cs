using UnityEngine;
using UnityEngine.UI;

namespace Ezerus.Trader
{
    public class TraderUIDescriptionPanel : MonoBehaviour
    {
        [SerializeField] private Text header;
        [SerializeField] private Text body;
        public void ConfigurePanel(Ezerus.Inventory.Item item)
        {
            header.text = item.Name;
            body.text = item.Description;
        }
    }
}
