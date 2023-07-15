using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    [SerializeField] private Slider _levelProgress;

    public void Init(int levelSize)
    {
        _levelProgress.maxValue = levelSize;
        _levelProgress.value = 0;
    }

    public void SetProgress(int progress)
    {
        _levelProgress.value = progress;
    }
}
