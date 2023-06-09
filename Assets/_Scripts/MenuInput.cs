using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Pause))]

public class MenuInput : MonoBehaviour
{
    [SerializeField] private Pause _pause;

    void Start()
    {
        _pause = GetComponent<Pause>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&(GameController.Game.GameState==GameController.GameStates.Map|| GameController.Game.GameState == GameController.GameStates.Combat))
        {
            _pause.SetPause();
        }
    }
}
