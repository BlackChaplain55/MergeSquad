using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProjectile : MonoBehaviour
{
    [SerializeField] private GameObject[] _projectilePool;
    [SerializeField] private Unit _unit;
    [SerializeField] private int _poolSize=5;
    [SerializeField] private GameObject _projectileTemplate;
    [SerializeField] private Transform _shootPoint;

    public void InitPool()
    {
        if (_projectileTemplate == null) return;
        _unit = GetComponent<Unit>();
        _projectilePool = new GameObject[_poolSize];
        for (int i=0; i<_poolSize; i++)
        {
            _projectilePool[i] = Instantiate(_projectileTemplate, gameObject.transform);
            _projectilePool[i].SetActive(false);
        }
    }
    public void ThrowProjectile()
    {
        if (_unit.currentEnemy != null)
        {
            foreach (GameObject projectileObject in _projectilePool)
            {
                if (!projectileObject.activeSelf)
                {
                    var projectile = projectileObject.GetComponent<Projectile>();
                    projectile.gameObject.SetActive(true);
                    projectile.Init(gameObject, _unit.currentEnemy.gameObject, _shootPoint);
                    break;
                }
            }
        }
    }
}
