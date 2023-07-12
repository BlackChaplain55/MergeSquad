using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject _archer;
    [SerializeField] private Unit _archerUnit;
    [SerializeField] private Unit _targetUnit;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _speed;
    [SerializeField] private float _ballisticHeight;

    private float _archerX;
    private float _targetX;

    public void Init(GameObject archer, GameObject target, Transform? shootPoint=null)
    {
        _archer = archer;
        _target = target;
        _archerUnit = archer.GetComponent<Unit>();
        _targetUnit = target.GetComponent<Unit>();
        if (shootPoint != null)
        {
            transform.position = shootPoint.position;
        }
        else
        {
            transform.position = _archer.transform.position;
        }
        DrawProjectile();
    }

    private void Update()
    {
        DrawProjectile();
    }

    private void DrawProjectile()
    {
        _archerX = _archer.transform.position.x;
        if (_targetUnit.State != UnitState.Die) _targetX = _target.transform.position.x;
        else gameObject.SetActive(false);
        var dist = _targetX - _archerX;
        var nextX = Mathf.MoveTowards(transform.position.x, _targetX, _speed * Time.deltaTime);
        var baseY = Mathf.Lerp(_archer.transform.position.y, _target.transform.position.y, (nextX - _archerX) / dist);
        var height = _ballisticHeight * (nextX - _archerX) * (nextX - _targetX) * (-0.5f * Mathf.Abs(dist));
        Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        if (transform.position == _target.transform.position)
        {
            _archerUnit.DealDamage();
            gameObject.SetActive(false);
        }
    }

    private Quaternion LookAtTarget(Vector2 rotation) {
            return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }
}
