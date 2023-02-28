using UnityEngine;

public class Trader : InteractableObject
{
    [SerializeField] private Ezerus.UI.TraderUI traderUI;
    private Ezerus.UI.TraderUI spawnedUI;
    private Ezerus.UI.UIRenderer interactUIRenderer;
    [SerializeField] private TraderItem[] Items;
    public override void OnInteractBeginToggle(InteractManager interactEntity)
    {
        spawnedUI = Instantiate(traderUI);
        interactUIRenderer = (interactEntity.AttachedEntity as Ezerus.UI.IUIHolder).UIRenderer;
        interactUIRenderer.EnableUIMode(true);
        interactUIRenderer.AttachUI(spawnedUI.RectTransform, true);
        interactEntity.AttachedEntity.AddRule(IRuleHandler.Rule.BlockCamera);
        interactEntity.AttachedEntity.AddRule(IRuleHandler.Rule.BlockMovement);
        print("Interact beggin");
    }
    public override void OnInteractEndToggle(InteractManager interactEntity)
    {
        interactUIRenderer.EnableUIMode(false);
        Player player = interactEntity.AttachedEntity as Player;
        
        interactEntity.AttachedEntity.RemoveRule(IRuleHandler.Rule.BlockCamera);
        interactEntity.AttachedEntity.RemoveRule(IRuleHandler.Rule.BlockMovement);
        interactUIRenderer = null;
        Destroy(spawnedUI.gameObject);
    }
    public struct TraderItem
    {
        public Ezerus.Inventory.Item PriceItem;
        public uint Price;
        public Ezerus.Inventory.Item Item;
    }
}
