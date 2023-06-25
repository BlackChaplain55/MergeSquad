using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitView))]

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitData _unitData;
    [SerializeField] private float _health;
    [field:SerializeField] public bool isEnemy { get; private set; }

    public float Position { get; private set; }
    public int Line { get; private set; }
    public int Level { get; private set; }
    public int Damage { get; private set; }
    public int AttackSpeed { get; private set; }
    
    public UnitState State;
    public Unit currentEnemy;

    private UnitView _view;
    //private UnitController _unitController;
    private UnitSpawner _unitSpawner;

    public Unit(bool isEnemy = false)
    {
        State = UnitState.Waiting;
    }

    public void Init(UnitSpawner unitSpawner)
    {
        _unitSpawner = unitSpawner;
        _view = GetComponent<UnitView>();
        Level = 1;
        Spawn();
    }

    public void Spawn()
    {
        State = UnitState.Walking;
        Line = Random.Range(0, CombatManager.Combat.linesCount);
        _health = _unitData.HP+_unitData.HPModifier*Level;
        _view.ChangeAnimation(State);
        transform.position = _unitSpawner.GetSpawnPoint(isEnemy);
        transform.Translate(new Vector3(0, Line * CombatManager.Combat.linesSpacing, 0));
        Position = transform.position.x;
        _view.FadeIn();
    }

    public void MoveUnit()
    {
        var direction = 1;
        if (isEnemy) direction = -1;
        State = UnitState.Attacking;
        _view.ChangeAnimation(State);
        transform.Translate(new Vector3(_unitData.WalkSpeed*CombatManager.Combat.walkSpeedMultiplier*direction, 0, 0));
        Position = transform.position.x;
    }

    public void Attack()
    {
        _view.ChangeAnimation(UnitState.Attacking);
    }

    public void DealDamage()
    {
        if (currentEnemy != null) currentEnemy.TakeDamage(_unitData.Damage);
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0) return;
        _health -= damage;
        if (_health < 0)
        {
            BeginDying();
        }
    }

    private void BeginDying()
    {
        State = UnitState.Die;
        
        _view.ChangeAnimation(UnitState.Die);
    }

    public void Die()
    {
        EventBus.onUnitDeath?.Invoke(this);
        Debug.Log("Unit " + gameObject + " is dead");
        _view.FadeOutAndRespawn();  
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCooldown());
    }

    private IEnumerator RespawnCooldown()
    {
        yield return new WaitForSeconds(_unitData.RespawnTime);
        Spawn();
    }

    public float GetAttackDistance()
    {
        return _unitData.AttackDistance;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}

public enum UnitTypes
{
    Warrior,
    Archer,
    Wizard
}

public enum UnitState
{
    Walking,
    Waiting,
    Attacking,
    Die
}