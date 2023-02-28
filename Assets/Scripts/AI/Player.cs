using Ezerus.UI;
using UnityEngine;

public class Player : Entity, IStaminaHolder, Ezerus.UI.IUIHolder
{
    [field:SerializeField] public StaminaHolder StaminaHolder { get; set; }
    [field:SerializeField] public UIRenderer UIRenderer {get; private set; }
    protected override void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    protected override void Update()
    {
    }
    protected override void OnDie()
    {
        Destroy(gameObject);
        base.OnDie();
    }
}
