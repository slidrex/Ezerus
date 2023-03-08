using UnityEngine;

public class StaminaHolder : MonoBehaviour
{
    [field: SerializeField] public float MaxStamina { get; private set; }
    [SerializeField] private float staminaRegenerationSpeed;
    [SerializeField] private float staminaRegenerationDelayTime;
    private float timeSinceStaminaConsumed;
    [field:SerializeField] public float CurrentStamina { get; private set; }
    private void Awake()
    {
        CurrentStamina = MaxStamina;
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
    public void AddStamina(float stamina) => CurrentStamina = Mathf.Clamp(CurrentStamina + stamina, 0.0f, MaxStamina);
    private void Update()
    {
        if(timeSinceStaminaConsumed < staminaRegenerationDelayTime)
        {
            timeSinceStaminaConsumed += Time.deltaTime;
        }else if(CurrentStamina < MaxStamina)
        {
            CurrentStamina = Mathf.Clamp(CurrentStamina += Time.deltaTime * staminaRegenerationSpeed, 0.0f, MaxStamina);
        }
    }
}
