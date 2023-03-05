using UnityEngine;

public class CooldownTableItem : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text cooldownText;
    private CooldownTable table;
    private System.Func<float> currentValue;
    public float MaxValue;
    public void Init(CooldownTable table, System.Func<float> value, float max)
    {
        currentValue = value;
        this.table = table;
        MaxValue = max;
    }
    private void Update()
    {
        float val = (float)System.Math.Round(MaxValue - (float)currentValue.Invoke(), 1);
        cooldownText.text = val.ToString();
        if(val <= 0.0f)
        {
            table.OnItemCooldowned(this);
        }
    }
}
