using UnityEngine;

public class PlayerBehaviour : Entity, IStaminaHolder
{
    [field:SerializeField] public StaminaHolder StaminaHolder { get; set; }

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
