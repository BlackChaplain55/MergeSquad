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
            bool showSkillSlots = GameController.Game.GameState == GameStates.Map;
            newCharUIController.Init(currentCharacter, showSkillSlots);
        }
    }
}
