using UnityEngine;

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
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (GameController.Game.GameState != GameStates.Menu)
        {
            _pause.SetPause();
        }
    }
}
