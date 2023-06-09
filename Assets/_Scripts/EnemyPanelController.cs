using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _characterUI;
    [SerializeField] private CharacterSO[] _enemies;

    public void Init()
    {
        if (_characterUI == null) _characterUI = Resources.Load<GameObject>("GamePrefabs/EnemyUI");
        foreach (CharacterSO currentCharacter in _enemies)
        {
            GameObject newCharUI = Instantiate(_characterUI, transform);
            var newCharUIController = newCharUI.GetComponent<CharController>();
            if(GameController.Game.GameState==GameController.GameStates.Map) newCharUIController.Init(currentCharacter,false);
            else newCharUIController.Init(currentCharacter, true);
        }
    }
}
