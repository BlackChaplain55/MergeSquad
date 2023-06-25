using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Image))]
public class UnitView : MonoBehaviour
{
    private string _walkingStateString="IsWalking";
    private string _waitingStateString = "IsWaiting";
    private string _attackStateString = "IsAttack";
    private string _dieStateString = "IsDie";

    private Animator _anim;
    private Image _image;
    private Unit _unit;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _unit = GetComponent<Unit>();
        _image = GetComponent<Image>();
    }

    public void ChangeAnimation(UnitState state)
    {
        _anim.SetBool(_walkingStateString, false);
        _anim.SetBool(_attackStateString, false);
        _anim.SetBool(_waitingStateString, false);
        _anim.SetBool(_dieStateString, false);

        switch (state){
            case UnitState.Walking: _anim.SetBool(_walkingStateString,true); break;
            case UnitState.Attacking: _anim.SetBool(_attackStateString,true); break;
            case UnitState.Waiting: _anim.SetBool(_waitingStateString,true); break;
            case UnitState.Die: _anim.SetBool(_dieStateString,true); break;
        }
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
