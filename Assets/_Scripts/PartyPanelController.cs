using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _characterUI;

    public void Init()
    {
        if (_characterUI == null) _characterUI = Resources.Load<GameObject>("GamePrefabs/CharacterUI");
        foreach (CharacterSO currentCharacter in GameController.Game.Party.Characters)
        {
            GameObject newCharUI = Instantiate(_characterUI, transform);
            var newCharUIController = newCharUI.GetComponent<CharController>();
            if(GameController.Game.GameState==GameController.GameStates.Map) newCharUIController.Init(currentCharacter,false);
            else newCharUIController.Init(currentCharacter, true);
        }
    }
}
