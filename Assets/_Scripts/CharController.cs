using UnityEngine;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    [SerializeField] private string _charName;
    [SerializeField] private CharacterSlot _armourSlot;
    [SerializeField] private CharacterSlot _weaponSlot;
    [SerializeField] private CharacterSlot _attackSlot;
    [SerializeField] private CharacterSlot _defendSlot;

    [SerializeField] private Text _charNameField;
    [SerializeField] private Text _hpText;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _portrait;

    [SerializeField] private int _maxHP;
    private int _HP;

    private void Awake()
    {
        _charNameField.text = _charName;
    }

    private void ShowHP()
    {
        _hpText.text = @"HP:"+_HP.ToString();
        _hpBar.fillAmount = _HP / _maxHP;
    }

    public void ChangeHP(int amount)
    {
        _HP += amount;
        Mathf.Clamp(_HP, 0, _maxHP);
    }

    public void Init(CharacterSO currentCharacter, bool showSkillSlots)
    {
        _maxHP = currentCharacter.HP;
        _HP = _maxHP;
        _charName = currentCharacter.CharName;
        _charNameField.text = _charName;
        //_weaponSlot.ItemTypes = currentCharacter.WeaponType;
        //_attackSlot.ItemTypes = currentCharacter.SkillType;
        //_defendSlot.ItemTypes = currentCharacter.SkillType;
        if(currentCharacter.visual != null) _portrait.sprite = currentCharacter.visual;
        if (!showSkillSlots)
        {
            _attackSlot.gameObject.SetActive(false);
            _defendSlot.gameObject.SetActive(false);
        }
    }
}
