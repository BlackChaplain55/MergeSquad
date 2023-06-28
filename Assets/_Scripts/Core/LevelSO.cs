using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Level_1", menuName = "Scriptables/LevelData")]
public class LevelSO : ScriptableObject
{
    public int Level = 1;
    public string Title = "���������";
    public string Description = "�������� ������";
    public AudioClip loadClip;
}