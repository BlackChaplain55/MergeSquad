using System;
using UnityEngine;

public abstract class ArtifactSO : ScriptableObject, IPrintableArtifact
{
    public Sprite Icon;
    public int MaxCount;
    public float MultiplierPerPiece;
    public string ArtifactName;
    [TextArea] public string ArtifactDescription;

    public virtual string GetKeyType => throw new NotImplementedException();

    public virtual string GetParameterType => throw new NotImplementedException();
}