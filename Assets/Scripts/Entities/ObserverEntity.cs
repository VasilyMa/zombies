using System;
using UnityEngine;

public class ObserverEntity : SourceEntity
{
    public event Action<float> OnExperienceChange;
    public event Action<float> OnElapsedTimeChange;
    public event Action<int> OnResourcesChange;
    public event Action<int> OnLevelChange;


    public static ObserverEntity instance;

    public override void Init()
    {
        instance = this;
    }

    public void ResourcesChange(int value)
    {
        OnResourcesChange?.Invoke(value);
    }

    public void ElapsedTimeChange(float value)
    {
        OnElapsedTimeChange?.Invoke(value);
    } 

    public void ExperienceChange(float normalized)
    {
        OnExperienceChange?.Invoke(normalized);
    }

    public void LevelChange(int level)
    {
        OnLevelChange?.Invoke(level);
    }
}
