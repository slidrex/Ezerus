using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [SerializeField] protected Entity Entity;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float gravityScale;
    [SerializeField] private Transform feetTransform;
    public float GravityScale { get; private set; }
    public bool IsGrounded { get; private set; }
    protected Vector3 MoveVector;
    protected float ProcessedGravity;
    [SerializeField] private LayerMask groundLayer;
    public float MovementSpeed { get; protected set; }
    [SerializeField] private float movementSpeed;
    protected bool BlockCamera { get; private set; }
    protected bool BlockMovement { get; private set; }
    protected Animator Animator;
    protected virtual void Awake()
    {
        Entity.RuleChangeCallback += OnRuleChanged;
        MovementSpeed = movementSpeed;
        Entity.RegisterAttribute((float v) => MovementSpeed = v, () => MovementSpeed, movementSpeed, EntityAttribute.Attribute.MovementSpeed);
        GravityScale = gravityScale;
    }
    private void OnRuleChanged(IRuleHandler.Rule rule, IRuleHandler.ChangeType changeType)
    {
        BlockCamera = Entity.ContainsRule(IRuleHandler.Rule.BlockCamera);
        BlockMovement = Entity.ContainsRule(IRuleHandler.Rule.BlockMovement);
    }
    ///<summary>Method for rotation handling if there is no active restrictions rules.</summary>
    protected virtual void HandleControllerRotation() {}
    protected virtual void Start()
    {
        Animator = GetComponent<Animator>();
    }
    protected virtual void Update()
    {
        if(BlockCamera == false)
            HandleControllerRotation();
    }
    protected void IsGroundCheck() => IsGrounded = Physics.CheckSphere(feetTransform.position, 0.3f, groundLayer);
    protected void HandlePhysics()
    {
        HandleGravity();
        ApplyGravity();
        if(BlockMovement == false)
            ApplyMovement();
    }
    private void HandleGravity()
    {
        if(IsGrounded == false)
        {
            ProcessedGravity -= GravityScale * Time.deltaTime;
        }
        else if(ProcessedGravity < 0) ProcessedGravity = 0.0f;
    }
    private void ApplyGravity() => characterController.Move(Vector3.up * ProcessedGravity * Time.deltaTime);
    private void ApplyMovement() => characterController.Move(MoveVector * Time.deltaTime);
}
