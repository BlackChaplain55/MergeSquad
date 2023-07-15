using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ArtifactUnit")]
public class ArtifactUnitSO : ArtifactSO, IPrintableArtifact
{
    public UnitType UnitType;
    public UnitParameterType UnitParameterType;

    public override string GetKeyType => UnitType.ToString();

    public override string GetParameterType => UnitParameterType.ToString();
}