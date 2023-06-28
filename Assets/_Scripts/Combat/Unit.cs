using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Coroutine AttackRoutine;
    public UnitState State;
    public Unit currentEnemy;

    private UnitView _view;
    private UnitSpawner _unitSpawner;

    private UnitStats _stats;
    public EquipmentSO WeaponSO { get; private set; }
    public EquipmentSO ArmorSO { get; private set; }
    public IUnitStatsProvider UnitStats { get; private set; }

    private void Awake()
    {
        IItemStatsProvider weaponStats = WeaponSO;
        IItemStatsProvider armorStats = ArmorSO;
        _stats = new UnitStats(1, _unitData.MaxHealth, _unitData);
        Artifact[] artifacts = ArtifactsRepository.UnitArtifacts[_stats.Type];

        UnitStats = new UnitLevelDecorator(_stats, _stats.Level);
        UnitStats = new ArtifactUnitDecorator(UnitStats, artifacts);
        armorStats = new ItemLevelDecorator(armorStats, _stats.Level);
        armorStats = new ArtifactItemDecorator(armorStats, artifacts);
        weaponStats = new ItemLevelDecorator(weaponStats, _stats.Level);
        weaponStats = new ArtifactItemDecorator(weaponStats, artifacts);
        UnitStats = new CombineUnitItemDecorator(UnitStats, weaponStats, armorStats);
    }

    public void Init(UnitSpawner unitSpawner, int level = 1)
    {
        _unitSpawner = unitSpawner;
        _view = GetComponent<UnitView>();
        _stats.Level = level;
        Spawn();
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
        State = UnitState.Attacking;
        _view.ChangeAnimation(State);
        transform.Translate(new Vector3(_unitData.WalkSpeed*CombatManager.Combat.walkSpeedMultiplier*direction, 0, 0));
        Position = transform.position.x;
    }

    public IEnumerator Attack(Unit target)
    {
        WaitForSeconds wait = new WaitForSeconds(_unitData.AttackSpeed);
        while (true)
        {
            currentEnemy = target;
            _view.ChangeAnimation(UnitState.Attacking);
            DealDamage();
            yield return wait;
        }
    }

    public void DealDamage()
    {
        if (currentEnemy != null) currentEnemy.TakeDamage(_unitData.Attack);
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

    public float GetAttackDistance()
    {
        return _unitData.AttackDistance;
    }

    public void Die()
    {
        EventBus.onUnitDeath?.Invoke(this);
        _view.FadeOutAndRespawn();  
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
}