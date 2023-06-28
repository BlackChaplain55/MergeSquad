using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] public List<Unit> UnitsList { get; private set; }
    [SerializeField] public List<Unit> EnemyList { get; private set; }
    [SerializeField] public float walkSpeedModifier { get; private set; }

    private void Awake()
    {
        UnitsList = new List<Unit>();
        EnemyList = new List<Unit>();
    }

    private void Update()
    {
        UpdateUnits(UnitsList);
        UpdateUnits(EnemyList);
    }

    public void AddUnitToList(Unit unit)
    {
        if (unit.isEnemy) EnemyList.Add(unit); else UnitsList.Add(unit);
    }

    public void RemoveFromList(Unit unit)
    {
        if (unit.isEnemy) EnemyList.Remove(unit); else UnitsList.Remove(unit);
    }

    private void UpdateUnits(List<Unit> unitList)
    {
        foreach (Unit unit in unitList)
        {
            if (unit.State == UnitState.Die)
                continue;

            var closestEnemy = GetClosestEnemy(unit);
            var distance = GetDistance(unit, closestEnemy);
            var attackDistance = unit.GetAttackDistance();
            if (distance > attackDistance)
            {
                unit.MoveUnit();
                if (unit.AttackRoutine != null)
                    StopCoroutine(unit.AttackRoutine);
            }
            else if (distance <= attackDistance)
            {
                unit.AttackRoutine = StartCoroutine(unit.Attack(closestEnemy));
            }
        }
    }

    private Unit GetClosestEnemy(Unit unit)
    {
        Unit closestEnemy = null;
        List<Unit> currentUnitList = null;
        if (EnemyList.Count > 0)
        {
            currentUnitList = unit.isEnemy ? UnitsList : EnemyList;
        }
        else if (unit.isEnemy && UnitsList.Count > 0)
        {
            currentUnitList = UnitsList;
        }
        if (currentUnitList == null) return closestEnemy;

        float minDistance = 10000;
        //closestEnemy = currentUnitList[0];
        foreach (Unit currentEnemy in currentUnitList)
        {
            if (currentEnemy.State != UnitState.Die)
                continue;

            float currentDistance = GetDistance(currentEnemy, unit);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                closestEnemy = currentEnemy;
            }
        }
        return closestEnemy;
    }

    private float GetDistance(Unit unit1, Unit unit2)
    {
        if (unit1 == null || unit2 == null)
        {
            return 10000;
        }
        else
        {
            return Mathf.Abs(unit1.Position - unit2.Position);
        }
    }
}

