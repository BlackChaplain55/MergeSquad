using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(UnitController))]
[RequireComponent(typeof(SpawnEffects))]

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] EnemyTamplates;
    [field:SerializeField] public GameObject[] _spawnPoints { get; private set; }
    [SerializeField] public List<GameObject> EnemySpawnPoints;
    [SerializedDictionary("Unit type","Prefab")] public SerializedDictionary<UnitType, GameObject> _unitTemplatesDictionary;
    [SerializeField] private UnitController _unitController;
    [SerializeField] private SpawnEffects _spawnEffects;
    private int _currentLine = 0;
    private int _currentEnemyLine = 0;


    private void Awake()
    {
        if (_unitController == null) _unitController = GetComponent<UnitController>();
        if (_spawnEffects == null) _spawnEffects = GetComponent<SpawnEffects>();
    }

    public GameObject GetUnitPrefab(UnitType type)
    {
        return _unitTemplatesDictionary.GetValueOrDefault(type);
    }

    public Transform GetSpawnPoint(bool isEnemy)
    {
        if (isEnemy)
        {
            var line = EnemySpawnPoints[_currentEnemyLine].transform;
            _currentEnemyLine++;
            if (_currentEnemyLine >= EnemySpawnPoints.Count) _currentEnemyLine = 0;
            return line;
        }
        else
        {
            var line = _spawnPoints[_currentLine].transform;
            _currentLine++;
            if (_currentLine >= _spawnPoints.Length) _currentLine = 0;
            return line;         
        }
        
        return transform;
    }

    public void SpawnEffect(Unit unit)
    {
        if (_spawnEffects == null) _spawnEffects = GetComponent<SpawnEffects>();
        _spawnEffects.SpawnEffect(unit.transform.parent, unit.isEnemy);
    }

    public void Spawn(GameObject unitObject, bool needInstance=true, int level =1)
    {
        Unit unit = unitObject.GetComponent<Unit>();
        Transform spawnPoint;
        int line = Random.Range(0, CombatManager.Combat.linesCount);
        GameObject newUnitObject;
        if (needInstance)
        {
            spawnPoint = GetSpawnPoint(unit.isEnemy);
            newUnitObject = Instantiate(unitObject, spawnPoint.position, spawnPoint.rotation, spawnPoint.transform);
        }
        else newUnitObject = unitObject;
        Unit newUnit = newUnitObject.GetComponent<Unit>();
        newUnit.Init(this,level);
        //newUnit.Init(this);
        _unitController.AddUnitToList(newUnit);
        //_spawnEffects.SpawnEffect(newUnit.transform.parent, newUnit.isEnemy);
    }

    public int GetInitialEnemyLevel()
    {
        return _unitController.InitialEnemyLevel;
    }
}
