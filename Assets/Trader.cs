using UnityEngine;
using Ezerus.Inventory;

namespace Ezerus.Trader
{
    public class Trader : InteractableObject
    {
        [SerializeField] private TraderUI traderUI;
        private TraderUI spawnedUI;
        private Ezerus.UI.UIRenderer interactUIRenderer;
        private Inventory.Inventory entityInventory;
        public System.Collections.Generic.List<TraderItem> Items;
        protected override void OnInteractEntityBeginToggle()
        {
            entityInventory = InteractEntity.GetComponent<Inventory.Inventory>();
            interactUIRenderer = (InteractEntity.AttachedEntity as Ezerus.UI.IUIHolder).UIRenderer;
            print(interactUIRenderer);
            if(interactUIRenderer.ContainsUI(UI.RenderUI.TraderMenu) == false)
            {
                spawnedUI = Instantiate(traderUI);
                interactUIRenderer.CacheUI(spawnedUI.RectTransform, UI.RenderUI.TraderMenu);
            } else spawnedUI = interactUIRenderer.GetUI(UI.RenderUI.TraderMenu).GetComponent<TraderUI>();
            interactUIRenderer.EnableUIMode(UI.RenderUI.TraderMenu);
            spawnedUI.ConfigureItems(this, Items);

            InteractEntity.AttachedEntity.AddRule(IRuleHandler.Rule.BlockCamera);
            InteractEntity.AttachedEntity.AddRule(IRuleHandler.Rule.BlockMovement);
            print("Interact beggin");
        }
        protected override void OnInteractEntityEndToggle()
        {
            interactUIRenderer.DisableUIMode();
            InteractEntity.AttachedEntity.RemoveRule(IRuleHandler.Rule.BlockCamera);
            InteractEntity.AttachedEntity.RemoveRule(IRuleHandler.Rule.BlockMovement);
            interactUIRenderer = null;
            entityInventory = null;
        }
        public bool CanBuy(Item priceItem, uint price)
        {
            if(entityInventory.GetItemsCount(priceItem) >= price)
            {
                return true;
            }
            
            return false;
        }
        public bool BuyItem(int index)
        {
            if(CanBuy(Items[index].PriceItem, Items[index].Price))
            {
                entityInventory.RemoveItems(Items[index].PriceItem, (int)Items[index].Price);
                entityInventory.AddItem(Items[index].SellItem);
                return true;
            }
            return false;
        }
        [System.Serializable]
        public struct TraderItem
        {
            public Ezerus.Inventory.Item PriceItem;
            public uint Price;
            public Ezerus.Inventory.Inventory.StackItem SellItem;
        }
    }
}
