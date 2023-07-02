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
    public float Position { get; private set; }
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

    [SerializeField] private UnitData _unitData;
    [SerializeField] private float _health;
    [SerializeField] private int _level;
    private UnitView _view;
    private UnitProjectile _projectiles;
    private UnitSpawner _unitSpawner;
    [SerializeField] private UnitStats _unitStats;

    private IUnitStatsProvider _statsProvider;

    public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangedEventHandler PropertyBeforeChanged;

    private void Awake()
    {
        IItemStatsProvider weaponStats;
        IItemStatsProvider armorStats;
        var artifactsRepo = GameController.Game.ArtifactsRepository;
        Artifact[] artifacts = artifactsRepo[UnitStats.Type];

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
        _view = GetComponent<UnitView>();
        if (TryGetComponent<UnitProjectile>(out _projectiles)) _projectiles.InitPool();
        Spawn();
    }

    public ref UnitStats GetUnitStatsRef() => ref _unitStats;

    public void SetWeapon(EquipmentSO weapon)
    {
        WeaponSO = weapon;
        //UnitStats.ReCompute(UnitParameterType.Attack, _statsProvider);
        //UnitStats.ReCompute(UnitParameterType.AttackSpeed, _statsProvider);
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
        State = UnitState.Walking;
        Line = Random.Range(0, CombatManager.Combat.linesCount);
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
        if(_projectiles!=null) _projectiles.ThrowProjectile();
    }

    /*
    public IEnumerator Attack(Unit target)
    {
        currentEnemy = target;
        while (true)
        {
            _view.ChangeAnimation(UnitState.Attacking);
            yield return new WaitForSeconds(UnitStats.AttackSpeed);
        }
    }
    */

    public void DealDamage()
    {
        if (currentEnemy != null) currentEnemy.TakeDamage(UnitStats.Attack);
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0) return;
        Health -= damage;
        if (Health < 0)
        {
            BeginDying();
        }
    }

    public float GetAttackDistance()
    {
        return _unitData.AttackDistance;
    }

    public void Die()
    {
        EventBus.onUnitDeath?.Invoke(this);
        _view.FadeOutAndRespawn();  
    }

    public void Upgrade()
    {
        Level++;
        float prevMaxHealth = _unitStats.MaxHealth;
        _unitStats.SetSnapshot(_statsProvider);
        //UnitStats.ReCompute(UnitParameterType.Attack, _statsProvider);
        //UnitStats.ReCompute(UnitParameterType.MaxHealth, _statsProvider);
        //UnitStats.ReCompute(UnitParameterType.AttackSpeed, _statsProvider);
        //UnitStats.ReCompute(UnitParameterType.UpgradeCost, _statsProvider);
        Health += _unitStats.MaxHealth - prevMaxHealth;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCooldown());
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

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}