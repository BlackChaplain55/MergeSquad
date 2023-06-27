using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;

[Serializable]
public struct GameProgress : INotifyPropertyChanged
{
    public int UnlockedLevel
    {
        get { return _unlockedLevel; }
        set { _unlockedLevel = value; }
    }
    public int CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }
    public Dictionary<UnitType, Dictionary<UnitParameterType, Artifact>> UnitArtifacts;
    public Dictionary<ItemType, Dictionary<ItemParameterType, Artifact>> ItemArtifacts;
    public event PropertyChangedEventHandler PropertyChanged;
    private int _unlockedLevel;
    private int _currentLevel;

    public GameProgress(
        int unlockedLevel = 1,
        int currentLevel = 1,
        Dictionary<UnitType, Dictionary<UnitParameterType, Artifact>> unitArtifacts = null,
        Dictionary<ItemType, Dictionary<ItemParameterType, Artifact>> itemArtifacts = null)
    {
        _unlockedLevel = unlockedLevel;
        _currentLevel = currentLevel;
        PropertyChanged = null;
        UnitArtifacts = unitArtifacts;
        ItemArtifacts = itemArtifacts;
    }

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}