using UnityEngine;

public class Player : Entity, IStaminaHolder
{
    [field:SerializeField] public StaminaHolder StaminaHolder { get; set; }
    public enum BlockingState
    {
        Blocked,
        Free
    }
    public BlockingState BlockState { get; set; } = BlockingState.Free;

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
