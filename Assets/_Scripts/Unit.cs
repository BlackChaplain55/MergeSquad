using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitView))]
[RequireComponent(typeof(UnitProjectile))]

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitData _unitData;
    [SerializeField] private float _health;
    [field:SerializeField] public bool isEnemy { get; private set; }

    public float Position { get; private set; }
    public int Line { get; private set; }
    public int Level { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }
    
    public UnitState State;
    public Unit currentEnemy;

    [SerializeField]  private UnitView _view;
    [SerializeField]  private UnitProjectile _projectiles;
    [SerializeField]  private UnitSpawner _unitSpawner;

    public Unit(bool isEnemy = false)
    {
        State = UnitState.Waiting;
    }

    public void Init(UnitSpawner unitSpawner)
    {
        _unitSpawner = unitSpawner;
        if (_view == null) _view = GetComponent<UnitView>();
        if (_projectiles == null) _projectiles = GetComponent<UnitProjectile>();
        if (_unitData.RangedAttack&&_projectiles!=null)
        {
            _projectiles.InitPool();
        }
        else
        {
            _projectiles.enabled = false;
        }
        Level = 0;
        Damage = _unitData.Damage;
        AttackSpeed = _unitData.AttackSpeed;
        Spawn();
    }

    public void Spawn()
    {
        State = UnitState.Walking;
        Line = Random.Range(0, CombatManager.Combat.linesCount);
        _health = _unitData.HP+_unitData.HPModifier*Level;
        _view.ChangeAnimation(State);
        _view.SetAttackSpeed(AttackSpeed);
        transform.position = _unitSpawner.GetSpawnPoint(isEnemy);
        transform.Translate(new Vector3(0, Line * CombatManager.Combat.linesSpacing, -1*Line));
        Position = transform.position.x;
        _view.FadeIn();
    }

    public void RaiseLevel()
    {
        Level++;
        Damage = _unitData.Damage + Level * _unitData.DamageModifier;
        AttackSpeed = _unitData.AttackSpeed + Level * _unitData.AttackSpeedModifier;
    }

    public void MoveUnit()
    {
        var direction = 1;
        if (isEnemy) direction = -1;
        State = UnitState.Walking;
        _view.ChangeAnimation(State);
        transform.Translate(new Vector3(_unitData.WalkSpeed*CombatManager.Combat.walkSpeedMultiplier*direction, 0, 0));
        Position = transform.position.x;
    }

    public void Attack()
    {
        _view.ChangeAnimation(UnitState.Attacking);
    }

    public void AttackRanged()
    {
        _projectiles.ThrowProjectile();
    }

    public void DealDamage()
    {
        if (currentEnemy != null)
        {
            currentEnemy.TakeDamage(_unitData.Damage);        
        }
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
    Wizard,
    Elite,
    Boss,
    Player
}

public enum UnitState
{
    Walking,
    Waiting,
    Attacking,
    Die
}