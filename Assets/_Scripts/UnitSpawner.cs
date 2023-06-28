using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(UnitController))]

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] EnemyTamplates;
    [SerializeField] public GameObject[] Lines { get; private set; }
    [SerializedDictionary("Unit type","Prefab")] public SerializedDictionary<UnitTypes, GameObject> _unitTemplatesDictionary;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private UnitController _unitController;

    private void Start()
    {
        if (_unitController == null) _unitController = GetComponent<UnitController>();
        Lines = new GameObject[CombatManager.Combat.linesCount];
        for(int i=0; i< CombatManager.Combat.linesCount; i++)
        {
            Lines[i] = new GameObject();
            Lines[i].transform.parent = transform;
        }
    }

    public GameObject GetUnitPrefab(UnitTypes type)
    {
        return _unitTemplatesDictionary.GetValueOrDefault(type);
    }

    public Transform GetLine(int line)
    {
        return Lines[line].transform;
    }

    public void Spawn(GameObject unitObject)
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
