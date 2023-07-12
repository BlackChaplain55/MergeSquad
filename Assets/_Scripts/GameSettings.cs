using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameSettings", menuName = "Scriptables/GameSettings")]
public class GameSettings : ScriptableObject
{
    public int MaxItemLevel = 1;
    public int ItemsBaseLevel = 1;
    public int ItemsPerKill;
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
    [SerializedDictionary(nameof(ItemType), nameof(ItemParameterType.MagicStrength) + "PerLevel")]
    public SerializedDictionary<ItemType, float> MagicStrengthPerLevel;
    [SerializedDictionary(nameof(ItemType), nameof(ItemParameterType.MagicRange) + "PerLevel")]
    public SerializedDictionary<ItemType, float> MagicRangePerLevel;
    public int ItemsDeathTimerPerLevel = 5;
    public List<Sprite> ItemUnderLayers;
    public List<Sprite> ItemBorders;

    [Header("Supported Magic")]
    public ItemType[] MagicTypes;

    public int GetMaxItemLevel() => MaxItemLevel - 1;
}