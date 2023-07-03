using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;

public class EnemySpawner : MonoBehaviour
{
    [SerializedDictionary("Unit type", "Prefab")] public SerializedDictionary<UnitType, GameObject> _enemyTemplatesDictionary;
    [SerializeField] private List<UnitType> _spawnUnitSequence;
    [SerializeField] private List<float> _spawnDelaySequence;
    [SerializeField] private UnitSpawner _unitSpawner;
    private int _UnitIndex;
    private int _DelayIndex;

    private int _currentSpawnedEnemy;

    private void Awake()
    {
        if (_unitSpawner == null) _unitSpawner = GetComponent<UnitSpawner>();
        _UnitIndex = 0;
        _DelayIndex = 0;
    }

    public GameObject GetUnitPrefab(UnitType type)
    {
        return _enemyTemplatesDictionary.GetValueOrDefault(type);
    }

    private IEnumerator SpawnWithDelay()
    {
        var delay = _spawnDelaySequence[_DelayIndex];
        yield return new WaitForSeconds(delay);
        var currentUnitType = _spawnUnitSequence[_UnitIndex];
        Spawn(_enemyTemplatesDictionary.GetValueOrDefault(currentUnitType));
        _DelayIndex++;
        if (_DelayIndex < _spawnDelaySequence.Count)
        {
            _UnitIndex++;
            if (_UnitIndex >= _spawnUnitSequence.Count) _UnitIndex = 0;
            StartCoroutine(SpawnWithDelay());
        }
    }

    private void Spawn(GameObject unitObject)
    {
        _unitSpawner.Spawn(unitObject);
    }

    [Button]
    public void BeginSpawnSequence()
    {
        StartCoroutine(SpawnWithDelay());
    }
}
