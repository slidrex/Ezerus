using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [SerializeField] protected Entity Entity;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float gravityScale;
    [SerializeField] private Transform feetTransform;
    public float GravityScale { get; private set; }
    public bool IsGrounded { get; private set; }
    protected Vector3 Velocity;
    [SerializeField] private LayerMask groundLayer;
    public float BaseMovementSpeed { get; protected set; }
    public float ResultMovementSpeed { get; protected set; }
    [SerializeField] private float movementSpeed;
    protected virtual void Awake()
    {
        BaseMovementSpeed = movementSpeed;
        ResultMovementSpeed = BaseMovementSpeed;
        GravityScale = gravityScale;
    }
    protected virtual void Start() {}
    protected virtual void Update() {}
    protected void IsGroundCheck() => IsGrounded = Physics.CheckSphere(feetTransform.position, 0.3f, groundLayer);
    protected void HandlePhysics()
    {
        HandleGravity();
        ApplyVelocity();
    }
    private void HandleGravity()
    {
        if(IsGrounded == false)
        {
            Velocity -= Vector3.up * GravityScale * Time.deltaTime;
        }
        else if(Velocity.y < 0) Velocity.y = 0.0f;
    }
    private void ApplyVelocity() => characterController.Move(Velocity * Time.deltaTime);
}
