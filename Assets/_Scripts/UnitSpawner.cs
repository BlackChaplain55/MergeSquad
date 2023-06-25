using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(UnitController))]

public class UnitSpawner : MonoBehaviour
{

    [SerializeField] public GameObject[] UnitTamplates;
    [SerializeField] public GameObject[] EnemyTamplates;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private UnitController _unitController;

    private void Start()
    {
        if (_unitController == null) _unitController = GetComponent<UnitController>();
    }

        [Button]
    private void SpawnRandomUnit()
    {
        int randomUnit = Random.Range(0, UnitTamplates.Length - 1);
        int randomEnemy = Random.Range(0, EnemyTamplates.Length - 1);
        Spawn(UnitTamplates[randomUnit]);
        Spawn(EnemyTamplates[randomEnemy]);
    }


    private void Spawn(GameObject unitObject)
    {
        Unit unit = unitObject.GetComponent<Unit>();
        Transform spawnPoint;
        if (unit.isEnemy)
        {
            spawnPoint = _enemySpawnPoint;
        }
        else {
            spawnPoint = _spawnPoint;
        }
            
        GameObject newUnitObject = Instantiate(unitObject, spawnPoint.position, spawnPoint.rotation, transform);
        Unit newUnit = newUnitObject.GetComponent<Unit>();
        newUnit.Init(this);
        _unitController.AddUnitToList(newUnit);
    }

    public Vector3 GetSpawnPoint(bool isEnemy)
    {
        if(isEnemy) return _enemySpawnPoint.position; else return _spawnPoint.position;
    }
}
