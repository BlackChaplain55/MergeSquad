using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewMagic_1", menuName = "ITEMS/Magic")]
public class MagicSO : ItemSO, IMagic
{
    ItemType IMagic.Type => Type;

    public bool Use()
    {
        return true;
    }
}
