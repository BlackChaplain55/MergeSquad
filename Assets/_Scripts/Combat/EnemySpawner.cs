using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(UnitSpawner))]

public class EnemySpawner : MonoBehaviour
{
    [SerializedDictionary("Unit type", "Prefab")] public SerializedDictionary<UnitType, GameObject> _enemyTemplatesDictionary;
    [SerializeField] private List<UnitType> _spawnUnitSequence;
    [SerializeField] private List<UnitType> _spawnDelaySequence;
    [SerializeField] private UnitSpawner _unitSpawner;
    private int _currentSpawnedEnemy;

    private void Start()
    {
        if (_unitSpawner == null) _unitSpawner = GetComponent<UnitSpawner>();
    }

    public GameObject GetUnitPrefab(UnitType type)
    {
        return _enemyTemplatesDictionary.GetValueOrDefault(type);
    }

    private IEnumerator SpawnWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(SpawnWithDelay(1));            
    }

    public void Spawn(GameObject unitObject)
    {
        _unitSpawner.Spawn(unitObject);
    }

    [Button]
    private void BeginSpawnSequence()
    {
        
    }
}
