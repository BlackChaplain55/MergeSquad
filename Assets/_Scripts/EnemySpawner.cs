using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(UnitSpawner))]

public class EnemySpawner : MonoBehaviour
{
    [SerializedDictionary("Unit type", "Prefab")] public SerializedDictionary<UnitTypes, GameObject> _enemyTemplatesDictionary;
    [SerializeField] private List<UnitTypes> _spawnUnitSequence;
    [SerializeField] private List<UnitTypes> _spawnDelaySequence;
    [SerializeField] private UnitSpawner _unitSpawner;
    private int _currentSpawnedEnemy;

    private void Start()
    {
        if (_unitSpawner == null) _unitSpawner = GetComponent<UnitSpawner>();
    }

    public GameObject GetUnitPrefab(UnitTypes type)
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
