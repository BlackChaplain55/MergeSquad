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

    [SerializeField] private LevelProgress _levelProgress;

    private void Awake()
    {
        UnitsList = new ObservableCollection<Unit>();
        EnemyList = new ObservableCollection<Unit>();
        if (_levelProgress == null) _levelProgress = GetComponent<LevelProgress>();
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
            if (distance > unit.GetAttackDistance()&&unit.State!=UnitState.Die)
            {
                if (unit.CanMove)
                {
                    if (unit.Position < _levelProgress.HeroLocalPosition) _levelProgress.ArmyMoving = true;
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
}

