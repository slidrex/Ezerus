using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    [SerializeField] private Level owner;
    [SerializeField] private Image expFill;
    [SerializeField] private Text level;

    private void Start()
    {
        LevelEvents.OnExpChanged += UpdateExpBar;
        LevelEvents.OnLevelUp += UpdateLevel;
    }
    public void UpdateExpBar()
    {
        expFill.fillAmount = (float)owner.GetCurrentExperience() / (float)owner.GetExperienceForNextLevel();
    }
    public void UpdateLevel()
    {
        level.text = owner.GetLevel().ToString();
    }
    private void OnDisable()
    {
        LevelEvents.OnExpChanged -= UpdateExpBar;
        LevelEvents.OnLevelUp -= UpdateLevel;
    }
}