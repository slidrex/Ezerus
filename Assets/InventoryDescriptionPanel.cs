using UnityEngine;
using UnityEngine.UI;

namespace Ezerus.Inventory
{
    public class InventoryDescriptionPanel : MonoBehaviour
    {
        [SerializeField] private Text nametag;
        [SerializeField] private Text body;
        public void ClearBody()
        {
            body.text = "";
        }
        public void SetName(string name, Color color)
        {
            nametag.text = name;
            nametag.color = color;
        }
        public void PushBodyLine(string text, Color color)
        {
            if(body.text != "") body.text += '\n';
            body.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";   
        }
        public void PushBodyLine(string firstWord, Color firstColor, string secondWord, Color secondColor)
        {
            if(body.text != "") body.text += '\n';
            body.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(firstColor)}>{firstWord}</color> ";
            body.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(secondColor)}>{secondWord}</color>";
        }
    }
}
