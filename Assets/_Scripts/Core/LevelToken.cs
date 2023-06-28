using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelToken : MonoBehaviour, IComparable
{
    [field: SerializeField] public LevelSO LevelData { get; private set; }
    [field: SerializeField] public Button ToBattleButton { get; private set; }
    [SerializeField] public AudioClip LoadingSFX;
    [SerializeField] private Color disabledTextColor;
    [SerializeField] private TextMeshProUGUI description;
    private TextMeshProUGUI _levelCounter;
    private Button _button;

    private void OnValidate()
    {
        Awake();
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _levelCounter = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetEnabled(bool flag)
    {
        _levelCounter.color = flag? Color.white : disabledTextColor;
        _button.interactable = flag;
    }

    public void SetLevel(int index)
    {
        LevelData = GetLevelSO(index);
        _levelCounter.SetText(LevelData.Level.ToString());
    }

    public int CompareTo(object obj)
    {
        if (obj is LevelToken levelToken)
        {
            if (levelToken == null) return 1;
            int myIndex = transform.GetSiblingIndex();
            int otherIndex = levelToken.transform.GetSiblingIndex();
            if (myIndex > otherIndex)
                return 1;
            else if (myIndex < otherIndex)
                return -1;
            else
                return 0;
        }
        return 0;
    }

    private LevelSO GetLevelSO(int index) => Resources.Load<LevelRepository>("Levels/LevelRepository")[index];
}
