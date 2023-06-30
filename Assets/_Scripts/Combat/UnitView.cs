using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Image))]
public class UnitView : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Image _image;
    [SerializeField] private Unit _unit;

    [SerializeField] private Sprite _walkingSprite;
    [SerializeField] private Sprite _waitingsprite;
    [SerializeField] private Sprite _attackSprite;
    [SerializeField] private Sprite _dieSprite;

    private string _walkingStateString="IsWalk";
    private string _waitingStateString = "IsWaiting";
    private string _attackStateString = "IsAttack";
    private string _dieStateString = "IsDie";

    private void Awake()
    {
        if (_anim == null) _anim = GetComponent<Animator>();
        if (_unit == null) _unit = GetComponent<Unit>();
        if (_image == null) _image = GetComponent<Image>();
    }

    public void ChangeAnimation(UnitState state)
    {
        _anim.SetBool(_walkingStateString, false);
        _anim.SetBool(_attackStateString, false);
        _anim.SetBool(_waitingStateString, false);
        _anim.SetBool(_dieStateString, false);

        switch (state){
            case UnitState.Walking:
                _anim.SetBool(_walkingStateString,true);
                _image.sprite = _walkingSprite;
                break;
            case UnitState.Attacking:
                _anim.SetBool(_attackStateString,true);
                _image.sprite = _attackSprite; 
                break;
            case UnitState.Waiting:
                _anim.SetBool(_waitingStateString,true);
                _image.sprite = _waitingsprite;
                break;
            case UnitState.Die:
                _anim.SetBool(_dieStateString,true);
                _image.sprite = _dieSprite;
                break;
        }
        _image.SetNativeSize();
    }

    public void SetAttackSpeed(float speed)
    {
        _anim.SetFloat("AttackSpeed", speed);
    }

    public void FadeIn()
    {
        _image.DOFade(1, 1);
    }

    public void FadeOutAndRespawn()
    {
        _image.DOFade(0, 1).OnComplete(() => { _unit.Respawn(); });   
    }
}
