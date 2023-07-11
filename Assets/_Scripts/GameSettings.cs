using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameSettings", menuName = "Scriptables/GameSettings")]
public class GameSettings : ScriptableObject
{
    public int MaxItemLevel;
    public int ItemsPerKill;
    public float RoundTime;
    public int StartSouls;
    public int SoulsPerKill;
    public float UnitSummonCostProgression;

    [Header("Items Settings")]
    [SerializedDictionary(nameof(ItemType), nameof(UnitParameterType.HealthPerLevel))]
    public SerializedDictionary<ItemType, float> HPPerLevel;
    [SerializedDictionary(nameof(ItemType), nameof(UnitParameterType.AttackPerLevel))]
    public SerializedDictionary<ItemType, float> AttackPerLevel;
    [SerializedDictionary(nameof(ItemType), nameof(UnitParameterType.AttackSpeedPerLevel))]
    public SerializedDictionary<ItemType, float> AttackSpeedPerLevel;
    public int ItemsDeathTimerPerLevel = 5;
    public List<Sprite> ItemUnderLayers;

    [Header("Supported Magic")]
    public ItemType[] MagicTypes; 
}