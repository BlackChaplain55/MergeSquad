using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewArmor", menuName = "ITEMS/Armor")]
public class ArmorSO : EquipmentSO
{
    private void OnEnable()
    {
        Type = ItemType.ArmorHeavy;
    }
}