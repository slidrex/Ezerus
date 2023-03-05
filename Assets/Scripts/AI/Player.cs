using Ezerus.UI;
using UnityEngine;

public class Player : Entity, IStaminaHolder, Ezerus.UI.IUIHolder
{
    [field:SerializeField] public StaminaHolder StaminaHolder { get; set; }
    [field:SerializeField] public UIRenderer UIRenderer {get; private set; }
    public Ezerus.Inventory.Inventory.StackItem Item;
    protected override void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Break();
          //  GetComponent<Ezerus.Inventory.Inventory>().AddItem(Item);
        }
    }
    protected override void OnDie()
    {
        Destroy(gameObject);
        base.OnDie();
    }
}
