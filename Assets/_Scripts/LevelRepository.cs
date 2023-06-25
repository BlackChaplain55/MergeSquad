using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelRepository", menuName = "Scriptables/LevelRepository")]
public class LevelRepository : ScriptableObject
{
    public List<LevelSO> Levels;
    public LevelSO this[int index]
    {
        get { return Levels[index]; }
        set { Levels[index] = value; }
    }

    private void OnValidate()
    {
        bool fileExists = true;
        int i = 1;
        Levels = new List<LevelSO>();
        do
        {
            LevelSO level = Resources.Load<LevelSO>("Levels/Level_" + i);
            fileExists = level != null;

            if (fileExists)
                Levels.Add(level);
            i++;

        } while (fileExists);
    }
}
