using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LevelProgress))]

public class UnitController : MonoBehaviour
{
    [SerializeField] public ObservableCollection<Unit> UnitsList { get; private set; }
    [SerializeField] public ObservableCollection<Unit> EnemyList { get; private set; }
    [SerializeField] public float walkSpeedModifier { get; private set; }
    public int InitialEnemyLevel { get; private set; }

    [SerializeField] private LevelProgress _levelProgress;

    [SerializeField] private float _secondsToUpgrade;
    [SerializeField] private int _maxLevelPerBoss;
    public int BossLevelPerStep;
    private int _maxLevel;
    public int MaxLevel { get { return _maxLevel; } }
    private bool _upgradeEnabled;

    private void Awake()
    {
        _maxLevel = _maxLevelPerBoss;
        InitialEnemyLevel = 1;
        UnitsList = new ObservableCollection<Unit>();
        EnemyList = new ObservableCollection<Unit>();
        if (_levelProgress == null) _levelProgress = GetComponent<LevelProgress>();
        _upgradeEnabled = true;
        StartCoroutine(UpgradeEnemies());
        EventBus.OnBossDeath += SetInitialEnemyLevel;
    }

    private void OnDestroy()
    {
        EventBus.OnBossDeath -= SetInitialEnemyLevel; ;
    }

    void Update()
    {
        UpdateUnits(UnitsList);
        UpdateUnits(EnemyList);
    }

    private void UpdateUnits(ObservableCollection<Unit> unitList)
    {
        foreach(Unit unit in unitList)
        {
            var closestEnemy = GetClosestEnemy(unit);
            var distance = GetDistance(unit,closestEnemy);
            if (distance > unit.GetAttackDistance()&&unit.State!=UnitState.Die||(unit.UnitReadonlyData.Type == UnitType.Hero&&_levelProgress.IsHeroMoving))
            {
                if (unit.CanMove)
                {
                    if (unit.Position < _levelProgress.HeroLocalPosition)
                        _levelProgress.ArmyMoving = true;
                    if (_levelProgress.ArmyMoving)
                    {
                        unit.MoveUnit();
                        //Debug.Log(unit.gameObject.ToString() + " Position " + unit.Position);
                    }
                    else if (unit.UnitReadonlyData.Type == UnitType.Hero)
                    {
                        unit.MoveUnit();
                    }
                    else
                    {
                        unit.SetIdle();
                        unit.UpdatePosition(unit.transform.position.x);
                    }
                }     
                else
                {
                    unit.SetIdle();
                    unit.UpdatePosition(unit.transform.position.x);
                }
            }
            else if (distance <= unit.GetAttackDistance() && unit.State != UnitState.Die&& !_levelProgress.IsHeroMoving)
            {
                unit.Attack();
                unit.currentEnemy = closestEnemy;
            }
        }
    }

    public IEnumerator UpgradeEnemies()
    {
        while (_upgradeEnabled)
        {
            yield return new WaitForSeconds(_secondsToUpgrade);
            foreach (Unit unit in EnemyList)
            {
                if (unit.Level< _maxLevel)
                    unit.Upgrade();
            }
        }
    }

    public void AddUnitToList(Unit unit)
    {
        if (unit.isEnemy) EnemyList.Add(unit); else UnitsList.Add(unit);
    }

    public void RemoveFromList(Unit unit)
    {
        if (unit.isEnemy) EnemyList.Remove(unit); else UnitsList.Remove(unit);
    }

    private Unit GetClosestEnemy(Unit unit)
    {
        var minDistance = 0f;
        Unit closestEnemy = null;
        List<Unit> currentUnitList = null;
        if (!unit.isEnemy&&EnemyList.Count > 0)
        {
            currentUnitList = EnemyList.ToList();      
        }
        else if (unit.isEnemy && UnitsList.Count > 0)
        {
            currentUnitList = UnitsList.ToList();
        }
        if (currentUnitList != null)
        {
            minDistance = 10000;
            foreach (Unit currentEnemy in currentUnitList)
            {
                if (currentEnemy.State != UnitState.Die)
                {
                    var currentDistance = GetDistance(currentEnemy, unit);
                    if (currentDistance < minDistance)
                    {
                        minDistance = currentDistance;
                        closestEnemy = currentEnemy;
                    }
                }
            }
        }
        return closestEnemy;
    }

    private float GetDistance(Unit unit1, Unit unit2)
    {
        if(unit1==null||unit2 == null)
        {
            return 10000;
        }
        else
        {
            return Mathf.Abs(unit1.Position - unit2.Position);
        }
    }

    private void SetInitialEnemyLevel()
    {
        _maxLevel += _maxLevelPerBoss;
        foreach (Unit enemy in EnemyList)
        {
            if (enemy.isEnemy && enemy.Level > InitialEnemyLevel) InitialEnemyLevel = enemy.Level;
        }
    }
}

