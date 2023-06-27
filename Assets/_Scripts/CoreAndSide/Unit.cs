using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private UnitStats _stats;
    public UnitSO UnitSO { get; private set; }
    public EquipmentSO WeaponSO { get; private set; }
    public EquipmentSO ArmorSO { get; private set; }
    public IUnitStatsProvider UnitStats { get; private set; }
    private void Awake()
    {
        IItemStatsProvider weaponStats = WeaponSO;
        IItemStatsProvider armorStats = ArmorSO;
        _stats = new UnitStats(1, UnitSO.MaxHealth, UnitSO);

        UnitStats = new UnitLevelDecorator(_stats, _stats.Level);
        UnitStats = new ArtifactUnitDecorator(UnitStats, ArtifactsRepository.UnitArtifacts[_stats.Type].Values.ToArray());
        armorStats = new ItemLevelDecorator(armorStats, _stats.Level);
        armorStats = new ArtifactItemDecorator(armorStats, ArtifactsRepository.UnitArtifacts[_stats.Type].Values.ToArray());
        weaponStats = new ItemLevelDecorator(weaponStats, _stats.Level);
        weaponStats = new ArtifactItemDecorator(weaponStats, ArtifactsRepository.UnitArtifacts[_stats.Type].Values.ToArray());
        UnitStats = new CombineUnitItemDecorator(UnitStats, weaponStats, armorStats);
    }
}