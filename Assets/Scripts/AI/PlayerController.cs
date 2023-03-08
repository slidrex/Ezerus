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
    private EntityAttribute.PercentClaim sprintClaim;
    protected new Player Entity { get => base.Entity as Player; }
    private bool blockSprint;
    private float cameraRotation;
    protected override void Awake()
    {
        base.Awake();
        Entity.RuleChangeCallback += RuleChangeCallback;
        staminaHolder = Entity as IStaminaHolder;
    }
    private void RuleChangeCallback(IRuleHandler.Rule rule, IRuleHandler.ChangeType changeType)
    {
        if(rule == IRuleHandler.Rule.BlockPlayerSprint)
        {
            blockSprint = Entity.ContainsRule(rule);
        }
    }
    protected override void Update()
    {
        base.Update();
        IsGroundCheck();
        Vector3 moveInput = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        
        HandleControllerInputs(moveInput);
        HandlePhysics();
    }
    private void HandleControllerInputs(Vector3 moveInput)
    {
        if(Input.GetAxisRaw("Vertical") > 0 && Input.GetKey(sprintKey))
        {
            if(Entity.ContainsRule(IRuleHandler.Rule.BlockPlayerSprint) == false && staminaHolder.StaminaHolder.TryConsume(Time.deltaTime * sprintStaminaConsumption))
            {
                if(sprintClaim.NullOrDeleted())
                {
                    sprintClaim = Entity.GetAttribute(EntityAttribute.Attribute.MovementSpeed).AddPercent(sprintSpeedMultiplier);
                    Animator.SetBool("IsSprint", true);
                }
            }
            else
            {
                Animator.SetBool("IsSprint", false);
                sprintClaim.TryRemove();
            }
        }
        else if(Input.GetKeyUp(sprintKey)) 
        {
            Animator.SetBool("IsSprint", false);
            sprintClaim.TryRemove();
        }

        MoveVector = new Vector3(moveInput.x * MovementSpeed, MoveVector.y, moveInput.z * MovementSpeed);

        Animator.SetInteger("MoveX", (int)MoveVector.x);
        Animator.SetInteger("MoveZ", (int)MoveVector.z);
        
        if(Input.GetKey(KeyCode.Space) && IsGrounded && BlockMovement == false) Jump();
    }
    private void Jump() => ProcessedGravity = jumpForce;
    protected override void HandleControllerRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector2 mouseInput = new Vector2(mouseX, mouseY) * mouseSensitivity;
        cameraRotation = Mathf.Clamp(cameraRotation - mouseInput.y, -90.0f, 90.0f);

        transform.Rotate(0.0f, mouseInput.x, 0.0f);
        _camera.transform.localRotation = Quaternion.Euler(cameraRotation, 0.0f, 0.0f);
    }
}
