using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    private bool _isPause;

    public void SetPause()
    {
        _isPause = !_isPause;
        if (_isPause)
        {
            Time.timeScale = 0;
            _pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            _pausePanel.SetActive(false);
        }
    }
}
