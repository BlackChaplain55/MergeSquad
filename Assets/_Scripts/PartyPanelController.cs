using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPanelController : MonoBehaviour
{
    [SerializeField] private GameObject _characterUI;
    [SerializeField] private PartyController party;

    private void OnValidate()
    {
        if (_characterUI == null) _characterUI = Resources.Load<GameObject>("GamePrefabs/CharacterUI");
    }

    public void Init()
    {
        foreach (CharacterSO currentCharacter in party.Characters)
        {
            GameObject newCharUI = Instantiate(_characterUI, transform);
            var newCharUIController = newCharUI.GetComponent<CharController>();
            newCharUIController.Init(currentCharacter, GameController.Game.GameState != GameStates.Map);
        }
    }
}
