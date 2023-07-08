using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(UnitView))]
public class Unit : MonoBehaviour, INotifyPropertyChanged
{
    [field:SerializeField] public bool isEnemy { get; private set; }
    public EquipmentSO WeaponSO { get; private set; }
    public EquipmentSO ArmorSO { get; private set; }
    public UnitStats UnitStats { get { return _unitStats; } }
    [field: SerializeField] public float Position { get; private set; }
    public int Line { get; private set; }
    public UnitData UnitReadonlyData
    {
        get { return _unitData; }
        set { _unitData = value; }
    }
    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            OnPropertyChanged();
        }
    }
    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            OnPropertyChanged();
        }
    }

    public UnitState State;
    public Unit currentEnemy;
    [field: SerializeField] public bool CanMove { get; private set; }

    [SerializeField] private UnitData _unitData;
    [SerializeField] private float _health;
    [SerializeField] private int _level;
    [SerializeField] private Unit _host;
    private UnitView _view;
    private UnitProjectile _projectiles;
    private UnitSpawner _unitSpawner;
    private bool _isBoss;

    [SerializeField] private UnitStats _unitStats;

    private IUnitStatsProvider _statsProvider;

    public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangedEventHandler PropertyBeforeChanged;

    private void Awake()
    {
        IItemStatsProvider weaponStats;
        IItemStatsProvider armorStats;
        if (GameController.Game != null)
        {
            var artifactsRepo = GameController.Game.ArtifactsRepository;
            Artifact[] artifacts = artifactsRepo[UnitStats.Type];
        }

        Level = 1;
        //UnitStats = new ArtifactUnitDecorator(_unitData, artifacts);
        _statsProvider = new UnitLevelDecorator(_unitData, this);
        //armorStats = new ArtifactItemDecorator(armorStats, artifacts);
        armorStats = new ArmorLevelDecorator(this);
        //weaponStats = new ArtifactItemDecorator(weaponStats, artifacts);
        weaponStats = new WeaponLevelDecorator(this);
        _statsProvider = new CombineUnitItemDecorator(_statsProvider, weaponStats, armorStats);
        _unitStats = new UnitStats(_unitData);
        _unitStats.SetSnapshot(_statsProvider);
        Health = UnitStats.MaxHealth;
    }

    public Unit(bool isEnemy = false)
    {
        State = UnitState.Waiting;
    }

    public void Init(UnitSpawner unitSpawner)
    {
        _unitSpawner = unitSpawner;
        transform.parent.parent.TryGetComponent<Unit>(out _host);
        Health = _unitData.BaseHealth;
        _view = GetComponent<UnitView>();
        _view.SetAttackSpeed(_unitData.AttackSpeed);
        _view.SetIdleSpeed();
        if (TryGetComponent<UnitProjectile>(out _projectiles)) _projectiles.InitPool();
        Position = transform.position.x;
        if (TryGetComponent<EnemySpawner>(out var _EnemySpawner))
        {
            _isBoss = true;
            CanMove = false;
            _EnemySpawner.SetSpawner(_unitSpawner);
        }
        else
        {
            _isBoss = false;
            CanMove = true;
        }
        if(_unitData.Type==UnitType.Hero) CanMove = false;
        
        Spawn();
    }

    public ref UnitStats GetUnitStatsRef() => ref _unitStats;

    public void SetWeapon(EquipmentSO weapon)
    {
        WeaponSO = weapon;
        _view.SetAttackSpeed(UnitStats.AttackSpeed);
        _unitStats.SetSnapshot(_statsProvider);
    }

    public void SetArmor(EquipmentSO armor)
    {
        ArmorSO = armor;
        float prevMaxHealth = UnitStats.MaxHealth;
        _unitStats.SetSnapshot(_statsProvider);
        float healthBonus = Mathf.Clamp(UnitStats.MaxHealth - prevMaxHealth, 0, float.MaxValue);
        Health = Mathf.Clamp(Health + healthBonus, 0, UnitStats.MaxHealth);
    }

    public void Spawn()
    {
        if (_host == null) return;
        if (_host.State == UnitState.Die) return;
        State = UnitState.Walking;
        Health = UnitStats.MaxHealth;
        Line = Random.Range(0, CombatManager.Combat.linesCount);
        _view.ChangeAnimation(State);
        transform.parent = _unitSpawner.GetSpawnPoint(Line, isEnemy);
        transform.position = transform.parent.position;
        //transform.Translate(new Vector3(0, Line * CombatManager.Combat.linesSpacing, 0));
        Position = transform.position.x;
        _view.UpdateHealth(1);
        _view.FadeIn();
    }

    public void MoveUnit()
    {
        var direction = 1;
        if (isEnemy) direction = -1;
        State = UnitState.Walking;
        _view.ChangeAnimation(State);
        transform.Translate(new Vector3(_unitData.WalkSpeed * CombatManager.Combat.walkSpeedMultiplier * direction, 0, 0));
        UpdatePosition(transform.position.x);
    }

    public void UpdatePosition(float position)
    {
        Position = position;
    }
    public void SetMove(bool value)
    {
        CanMove = value;
    }

    public void SetIdle()
    {
        _view.ChangeAnimation(UnitState.Waiting);
    }

    public void Attack()
    {
        _view.ChangeAnimation(UnitState.Attacking);
    }

    public void AttackRanged()
    {
        if(_projectiles!=null) _projectiles.ThrowProjectile();
    }

    public void DealDamage()
    {
        if (currentEnemy != null) currentEnemy.TakeDamage(UnitStats.Attack);
    }

    public void TakeDamage(float damage)
    {
        if (State == UnitState.Die) return;
        if (damage < 0) return;
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            BeginDying();
        }
        _view.UpdateHealth(Health/_unitStats.MaxHealth);
        _view.DamageEffect();
    }

    public float GetAttackDistance()
    {
        return _unitData.AttackDistance;
    }

    public void Die()
    {
        if (_unitData.Type == UnitType.Hero)
        {
            EventBus.onHeroDeath?.Invoke();
        }
        else
        {
            EventBus.onUnitDeath?.Invoke(this);
            _view.FadeOutAndRespawn();
        }
    }

    public void Upgrade()
    {
        Level++;
        float prevMaxHealth = _unitStats.MaxHealth;
        _unitStats.SetSnapshot(_statsProvider);
        Health += _unitStats.MaxHealth - prevMaxHealth;
    }

    public void Respawn()
    {
        if (!_isBoss) StartCoroutine(RespawnCooldown()); else EventBus.onBossDeath?.Invoke(this);
    }

    private void BeginDying()
    {
        State = UnitState.Die;
        
        _view.ChangeAnimation(UnitState.Die);
    }

    private IEnumerator RespawnCooldown()
    {
        yield return new WaitForSeconds(_unitData.RespawnTime);
        Spawn();
    }


    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}