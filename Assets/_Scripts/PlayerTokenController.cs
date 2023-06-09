using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class PlayerTokenController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private Transform _canvasTransform;
    private Vector3 _target;

    void Start()
    {
        _target = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _target = getPointerPosition();
        }
        transform.position = Vector3.MoveTowards(transform.position, _target, _moveSpeed);
    }

    private Vector3 getPointerPosition()
    {
        Vector3 pointerPosition = Input.mousePosition;
        return pointerPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyToken enemyToken;
        if (collision.gameObject.TryGetComponent<EnemyToken>(out enemyToken))
        {
            BeginBattle(enemyToken);
        };
    }

    private void BeginBattle(EnemyToken enemyToken)
    {
        GameController.Game.StopMusic();
        Menu menu = GameObject.Find("GameMenu").GetComponent<Menu>();
        menu.StartScene(GameController.GameStates.Combat, enemyToken.AttackClip);
    }
}
