using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(UnitController))]

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] EnemyTamplates;
    [field:SerializeField] public GameObject[] _spawnPoints { get; private set; }
    [SerializeField] public List<GameObject> EnemySpawnPoints;
    [SerializedDictionary("Unit type","Prefab")] public SerializedDictionary<UnitType, GameObject> _unitTemplatesDictionary;
    //[SerializeField] private Transform _spawnPoint;
    //[SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private UnitController _unitController;

    private void Start()
    {
        if (_unitController == null) _unitController = GetComponent<UnitController>();
        //Lines = new GameObject[CombatManager.Combat.linesCount];
    }

    public GameObject GetUnitPrefab(UnitType type)
    {
        return _unitTemplatesDictionary.GetValueOrDefault(type);
    }

    public Transform GetSpawnPoint(int line, bool isEnemy)
    {
        if (isEnemy)
        {
            if(line<= EnemySpawnPoints.Count)
            {
                return EnemySpawnPoints[line].transform;
            }
            else
            {
                return EnemySpawnPoints[0].transform;
            }
        }
        else
        {
            if (line <= _spawnPoints.Length)
            {
                return _spawnPoints[line].transform;
            }
            else
            {
                return _spawnPoints[0].transform;
            }
        }
        return null;
    }

    public void Spawn(GameObject unitObject, bool needInstance=true)
    {
        Unit unit = unitObject.GetComponent<Unit>();
        Transform spawnPoint;
        int line = Random.Range(0, CombatManager.Combat.linesCount);
        spawnPoint = GetSpawnPoint(line, unit.isEnemy);
        GameObject newUnitObject;
        if (needInstance) newUnitObject = Instantiate(unitObject, spawnPoint.position, spawnPoint.rotation, spawnPoint.transform);
        else newUnitObject = unitObject;
        Unit newUnit = newUnitObject.GetComponent<Unit>();
        newUnit.Init(this);
        _unitController.AddUnitToList(newUnit);
    }
}
