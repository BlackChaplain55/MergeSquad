using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LevelView))]

public class LevelProgress : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private UnitController _unitConroller;
    [SerializeField] private LevelView _levelProgressView;
    //[SerializeField] private List<EnemySpawner> _bosses;
    [SerializeField] private List<GameObject> _bossesTemplates;
    //[SerializeField] private GameObject _currentBoss;
    [SerializeField] private Canvas _currentCanvas;
    //[SerializeField] private EnemySpawner _currentBoss;
    [SerializeField] private Hero _hero;
    private int _levelStep;
    private float _levelPosition;
    private float _levelInitialPosition;
    private float _heroPosition;
    private float _levelLength;
    [SerializeField] private float _bossOffsetY;
    [SerializeField] private float _bossOffsetX;
    
    private bool _heroMoving;
    public bool IsHeroMoving { get { return _heroMoving; } }
    public bool ArmyMoving;
    public float HeroLocalPosition { get { return _hero.transform.position.x; } }
    public float LevelPosition { get { return _levelPosition; } }
    public float LevelLength { get { return _levelLength; } }


    // Start is called before the first frame update
    private void Start()
    {
        if (_unitSpawner == null) _unitSpawner = GetComponent<UnitSpawner>();
        if (_unitConroller == null) _unitConroller = GetComponent<UnitController>();
        if (_levelProgressView == null) _levelProgressView = GetComponent<LevelView>();        
        EventBus.OnBossDeath += BossDeath;
        Init();
        _levelProgressView.Init(_bossesTemplates.Count);
    }

    private void OnDestroy()
    {
        EventBus.OnBossDeath -= BossDeath;
    }

    public void Init()
    {
        _levelLength = _currentCanvas.renderingDisplaySize.x;
        _levelInitialPosition = _hero.transform.localPosition.x;
        if (_bossesTemplates.Count > 0)
        {
            _levelStep = 0;
            InitCurrentBoss();
        }
        InitHero();
    }

    private void InitHero()
    {
        _heroMoving = false;
        ArmyMoving = true;
        _hero.SetMove(_heroMoving);
        _hero.Init(_unitSpawner);
        _unitConroller.AddUnitToList(_hero);
    }

    private void Update()
    {
        if (_heroMoving)
        {
            ShiftLevel();
            EventBus.onHeroMove?.Invoke(new Vector2(_levelPosition, 0));
        }
    }

    private void ShiftLevel() 
    {      
        if (_hero.transform.localPosition.x >= _heroPosition)
        {
            _heroMoving = false;
            _hero.SetMove(_heroMoving);
        }
        _levelPosition = _hero.transform.localPosition.x - _levelInitialPosition;
        foreach (Unit unit in _unitConroller.UnitsList)
        {
            if (unit.UnitReadonlyData.Type != UnitType.Hero)
            {
                unit.transform.Translate(-_hero.UnitReadonlyData.WalkSpeed * CombatManager.Combat.walkSpeedMultiplier*Time.deltaTime, 0, 0);
                unit.UpdatePosition(unit.transform.position.x);
            }
        }
        transform.parent.localPosition = new Vector2(-_levelPosition, 0);  
    }

    private void BossDeath()
    {
        _levelStep++;
        
        _heroPosition = _hero.transform.localPosition.x + _levelLength;
        _heroMoving = true;
        ArmyMoving = false;
        _hero.SetMove(_heroMoving);
        if (_levelStep< _bossesTemplates.Count)
        {
            InitCurrentBoss();
            _levelProgressView.SetProgress(_levelStep);
        }
        else
        {
            EventBus.OnFinalBossDeath?.Invoke();
        }
        
    }

    public void InitCurrentBoss()
    {
        GameObject boss = Instantiate(_bossesTemplates[_levelStep],transform.parent);
        boss.transform.localPosition = new Vector3(_levelLength/2-_bossOffsetX+_levelStep*_levelLength,_bossOffsetY,0);
        Unit bossUnit = boss.GetComponent<Unit>();
        EnemySpawner bossSpawner = boss.GetComponent<EnemySpawner>();
        bossUnit.Init(_unitSpawner);
        _unitConroller.AddUnitToList(bossUnit);
        _unitSpawner.EnemySpawnPoints.Clear();
        for(int i=0; i < boss.transform.childCount; i++)
        {
            _unitSpawner.EnemySpawnPoints = bossSpawner.SpawnPoints;
        }
        bossSpawner.BeginSpawnSequence();
        _hero.transform.SetAsLastSibling();
    }

}

