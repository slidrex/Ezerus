using UnityEngine;

namespace Ezerus.Inventory
{
    public static class ItemDisplayExtension
    {
        public static Color GetRarityColor(this Item.Rarity rarity)
        {
            switch(rarity)
            {
                case Item.Rarity.Common: return Color.grey;
                case Item.Rarity.Rare: return Color.blue;
                case Item.Rarity.Mythic: return Color.red;
                case Item.Rarity.Exotic: return Color.yellow;
            }
            return Color.white;
        }
        public static Color GetTypeColor(this Item.Type type)
        {
            switch(type)
            {
                case Item.Type.Material: return Color.yellow;
                case Item.Type.Sword: return Color.grey;
                case Item.Type.Ability: return Color.magenta;
                case Item.Type.Money: return Color.green;
                case Item.Type.Staff: return Color.blue;
            }
            return Color.white;
        }
    }
}
