using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

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
        int i = 1;
        LevelSO level = null;
        Levels = new List<LevelSO>();
        do
        {
            level = Resources.Load<LevelSO>("Levels/Level_" + i);

            if (level != null)
                Levels.Add(level);
            i++;
        } while (level != null);
    }
}
