using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    [SerializeField] private Image _nextRoundTimerVisual;
    [SerializeField] private float _timer = 0;
    [SerializeField] private float _timerMax = 20;
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private int _enemiesCount;
    [SerializeField] private MergeItems _mergeItems;

    private void Awake()
    {
        if (GameController.Game!=null) _timerMax = GameController.Game.Settings.RoundTime;
        _timer = _timerMax;
        if (_mergeItems == null) _mergeItems = GetComponent<MergeItems>();
    }

    void Update()
    {
        if( _timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _timer = _timerMax;
            CalculateRound();
            _mergeItems.PlaceRandomItem();
        }
        _nextRoundTimerVisual.fillAmount = _timer / _timerMax;
    }

    public void InitiateBattle()
    {

    }


    private void CalculateRound()
    {

    }
}
