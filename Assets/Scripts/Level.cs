using UnityEngine;
public class LevelEvents
{
    public static System.Action OnExpChanged;
    public static System.Action OnLevelUp;
}
public class Level : MonoBehaviour
{
    [SerializeField] private byte level = 1;
    [SerializeField] private int currentExperience = 0;
    [SerializeField] private int experienceForNextLevel = 10;
    [SerializeField] private int test;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U)) AddExperience(test);
    }
    public void LevelUp(byte add)
    {
        level += add;
        for (int i = 0; i < add; i++)
        {
            experienceForNextLevel += experienceForNextLevel / 2;
        }
        LevelEvents.OnLevelUp?.Invoke();
    }
    public void AddExperience(int experience)
    {
        currentExperience += experience;
        if(currentExperience >= experienceForNextLevel)
        {
            byte lvl = (byte)(currentExperience / experienceForNextLevel);
            int experienceRemainder = (int)(currentExperience % experienceForNextLevel);
            currentExperience = experienceRemainder;
            LevelUp(lvl);
        }
        LevelEvents.OnExpChanged?.Invoke();
    }
    public byte GetLevel() => level;
    public int GetCurrentExperience() => currentExperience;
    public int GetExperienceForNextLevel() => experienceForNextLevel;
}
