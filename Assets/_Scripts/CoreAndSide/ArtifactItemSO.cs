using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ArtifactItem")]
public class ArtifactItemSO : ArtifactSO, IPrintableArtifact
{
    public ItemType ItemType;
    public ItemParameterType ItemParameterType;

    public override string GetKeyType => ItemType.ToString();

    public override string GetParameterType => ItemParameterType.ToString();
}
