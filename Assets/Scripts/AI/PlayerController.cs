using UnityEngine;

public class PlayerController : EntityMovement
{
    private IStaminaHolder staminaHolder;
    [SerializeField] private Camera _camera;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sprintStaminaConsumption;
    [SerializeField] private KeyCode sprintKey;
    [SerializeField] private float sprintSpeedMultiplier;
    protected new PlayerBehaviour Entity { get => base.Entity as PlayerBehaviour; }
    private float cameraRotation;
    protected override void Awake()
    {
        base.Awake();
        staminaHolder = Entity as IStaminaHolder;
    }
    protected override void Update()
    {
        IsGroundCheck();
        Vector3 moveInput = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        if(Entity.BlockState != PlayerBehaviour.BlockingState.Blocked)
        {
            HandleCamera();
            
        }else
            moveInput = Vector3.zero;
        HandleControllerInputs(moveInput);
        HandlePhysics();
    }
    private void HandleControllerInputs(Vector3 moveInput)
    {
        ResultMovementSpeed = BaseMovementSpeed;
        if(Input.GetAxisRaw("Vertical") > 0 && Input.GetKey(sprintKey))
        {
            if(staminaHolder.StaminaHolder.TryConsume(Time.deltaTime * sprintStaminaConsumption))
            {
                ResultMovementSpeed = BaseMovementSpeed * sprintSpeedMultiplier;
            }
        }
        Velocity = new Vector3(moveInput.x * ResultMovementSpeed, Velocity.y, moveInput.z * ResultMovementSpeed);
        
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded) Jump();
    }
    private void Jump() => Velocity.y = jumpForce;
    private void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector2 mouseInput = new Vector2(mouseX, mouseY) * mouseSensitivity;
        cameraRotation = Mathf.Clamp(cameraRotation - mouseInput.y, -90.0f, 90.0f);

        transform.Rotate(0.0f, mouseInput.x, 0.0f);
        _camera.transform.localRotation = Quaternion.Euler(cameraRotation, 0.0f, 0.0f);
    }
}
