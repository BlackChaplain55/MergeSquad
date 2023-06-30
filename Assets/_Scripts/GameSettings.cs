using UnityEngine;

[CreateAssetMenu(fileName = "NewGameSettings", menuName = "Scriptables/GameSettings")]
public class GameSettings : ScriptableObject
{
    public int MaxItemLevel;
    public int ItemsPerRound;
    public float RoundTime;
}
