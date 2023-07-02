using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private UnitController _unitConroller;
    [SerializeField] private List<EnemySpawner> _bosses;
    [SerializeField] private EnemySpawner _currentBoss;
    [SerializeField] private Unit _hero;
    [SerializeField] private float levelPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (_unitSpawner == null) _unitSpawner = GetComponent<UnitSpawner>();
        if (_unitConroller == null) _unitConroller = GetComponent<UnitController>();
        Init();
    }

    public void Init()
    {
        if (_bosses.Count > 0)
        {
            if (_currentBoss == null)
            {
                _currentBoss = _bosses[0];
                InitCurrentBoss();
            }
        }
    }

    private void InitHero()
    {
        _hero.Init(_unitSpawner);
        _unitConroller.AddUnitToList(_hero);
    }

    public void InitCurrentBoss()
    {
        Unit bossUnit = _currentBoss.gameObject.GetComponent<Unit>();
        bossUnit.Init(_unitSpawner);
        _unitConroller.AddUnitToList(bossUnit);
        _currentBoss.BeginSpawnSequence();
    }

}

