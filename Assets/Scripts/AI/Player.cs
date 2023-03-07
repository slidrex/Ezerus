using Ezerus.UI;
using UnityEngine;

public class Player : Entity, IStaminaHolder, Ezerus.UI.IUIHolder, ICooldownTableHandler
{
    [field:SerializeField] public StaminaHolder StaminaHolder { get; set; }
    [field:SerializeField] public UIRenderer UIRenderer {get; private set; }

    [field: SerializeField] public CooldownTable Table {get; private set;}

    public Ezerus.Inventory.Inventory.StackItem Item;
    private EntityAttribute.PercentClaim claim;
    protected override void Start()
    {
        base.Start();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Break();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            TryGetAttribute(EntityAttribute.Attribute.MovementSpeed, out EntityAttribute attribute);
            claim = attribute.AddPercent(-0.2f);
        }
        if(Input.GetKeyUp(KeyCode.M))
        {
            claim.Remove();
        }
    }
    protected override void OnDie()
    {
        Destroy(gameObject);
        base.OnDie();
    }
}
