using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Level_1", menuName = "Scriptables/LevelData")]
public class LevelSO : ScriptableObject
{
    public int Level = 1;
    public string Title = "Заголовок";
    public string Description = "Описание уровня";
    public AudioClip loadClip;
}