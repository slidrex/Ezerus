using UnityEngine;

public class StaminaHolder : MonoBehaviour
{
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegenerationSpeed;
    [SerializeField] private float staminaRegenerationDelayTime;
    private float timeSinceStaminaConsumed;
    [field:SerializeField] public float CurrentStamina { get; set; }
    private void Awake()
    {
        CurrentStamina = maxStamina;
    }
    public bool TryConsume(float consumption)
    {
        if(CurrentStamina - consumption >= 0)
        {
            timeSinceStaminaConsumed = 0.0f;
            CurrentStamina -= consumption;
            return true;
        }
        return false;
    }
    private void Update()
    {
        if(timeSinceStaminaConsumed < staminaRegenerationDelayTime)
        {
            timeSinceStaminaConsumed += Time.deltaTime;
        }else if(CurrentStamina < maxStamina)
        {
            CurrentStamina = Mathf.Clamp(CurrentStamina += Time.deltaTime * staminaRegenerationSpeed, 0.0f, maxStamina);
        }
    }
}
