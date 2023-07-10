using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="NewWeapon", menuName = "ITEMS/Weapon")]
public class WeaponSO : EquipmentSO
{
    private void OnEnable()
    {
        Type = ItemType.Melee;
    }
}