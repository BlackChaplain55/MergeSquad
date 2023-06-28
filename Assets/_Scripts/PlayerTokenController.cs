using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class PlayerTokenController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _moveTimeStep = 0.02f;
    [SerializeField] private Transform _canvasTransform;
    private Coroutine _playerMoveRoutine;
    private Vector3 _target;

    private void Awake()
    {
        _target = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _target = GetPointerPosition();
        }
        transform.position = Vector3.MoveTowards(transform.position, _target, _moveSpeed);
    }

    private Vector3 GetPointerPosition()
    {
        Vector3 pointerPosition = Input.mousePosition;
        return pointerPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out LevelToken enemyToken))
        {
            BeginBattle(enemyToken);
        };
    }

    private void BeginBattle(LevelToken levelToken)
    {
        Menu menu = GameObject.Find("GameMenu").GetComponent<Menu>();
        menu.StartScene(GameStates.Combat, levelToken.LoadingSFX);
    }

    /*
    private void OnMoveCommand(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            if (_playerMoveRoutine != null)
                StopCoroutine(_playerMoveRoutine);

            _playerMoveRoutine = StartCoroutine(MovePlayer(_moveTimeStep));
        }
    }
    */

    private IEnumerator MovePlayer(float timeStep)
    {
        WaitForSeconds wait = new(timeStep);
        float distance;

        while (true)
        {
            distance = (_target - transform.position).magnitude;

            if (distance < 0.5f) break;
            transform.position = Vector3.MoveTowards(transform.position, _target, _moveSpeed);

            yield return wait;
        }
    }
}