using UnityEngine;

[CreateAssetMenu(fileName = "NewMagic_1", menuName = "ITEMS/Magic")]
public class MagicSO : ItemSO
{
    public float CooldownTime;
    public float BaseStrength;
    public float BaseRange;
}