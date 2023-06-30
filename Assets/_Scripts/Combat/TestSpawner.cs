using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TestSpawner : MonoBehaviour
{

    [SerializeField] public UnitSpawner Spawner;
    // Start is called before the first frame update

    private void Start()
    {
        if (Spawner == null) GetComponent<UnitSpawner>();
    }

    [Button]
    private void SpawnRandomEnemyUnit()
    {
        int randomEnemy = Random.Range(0, Spawner.EnemyTamplates.Length);
        Spawner.Spawn(Spawner.EnemyTamplates[randomEnemy]);
    }

    [Button]
    private void SpawnArcher()
    {
        var unitPrefab = Spawner.GetUnitPrefab(UnitType.Archer);
        if (unitPrefab != null) Spawner.Spawn(unitPrefab);
    }

    [Button]
    private void SpawnWarrior()
    {
        var unitPrefab = Spawner.GetUnitPrefab(UnitType.Warrior);
        if (unitPrefab != null) Spawner.Spawn(unitPrefab);
    }
}
