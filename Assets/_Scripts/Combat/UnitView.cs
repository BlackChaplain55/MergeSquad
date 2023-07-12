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
    [SerializeField] private UnitHealtBar _healthBar;

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
        if (_healthBar == null) _healthBar = GetComponentInChildren<UnitHealtBar>();
        if (_anim == null) _anim = GetComponent<Animator>();
        if (_unit == null) _unit = GetComponent<Unit>();
        if (_image == null) _image = GetComponent<Image>();
        if (_healthBar != null) _healthBar.Init(_image.rectTransform.sizeDelta);
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
                //_image.sprite = _walkingSprite;
                break;
            case UnitState.Attacking:
                _anim.SetBool(_attackStateString,true);
                //_image.sprite = _attackSprite; 
                break;
            case UnitState.Waiting:
                _anim.SetBool(_waitingStateString,true);
                //_image.sprite = _waitingsprite;
                break;
            case UnitState.Die:
                _anim.SetBool(_dieStateString,true);
                //_image.sprite = _dieSprite;
                break;
        }
        //_image.SetNativeSize();
    }

    public void SetAttackSpeed(float speed)
    {
        var speedFluctuation = Random.Range(-0.05f, 0.05f);
        _anim.SetFloat("AttackSpeed", speed+ speedFluctuation);
    }
    public void SetIdleSpeed()
    {
        var speed = Random.Range(0.7f, 1.3f);
        _anim.SetFloat("IdleSpeed", speed);
    }

    public void FadeIn()
    {
        _image.DOFade(1, 2);
        _healthBar.gameObject.SetActive(true);
    }

    public void FadeOutAndRespawn()
    {
        _healthBar.gameObject.SetActive(false);
        _image.DOKill();
        _image.DOFade(0, 1).OnComplete(() => { _unit.Respawn(); });   
    }

    public void UpdateHealth(float amount)
    {
        _healthBar.SetHealthBar(amount);
    }

    public void DamageEffect()
    {
        var damageEffectSeq = DOTween.Sequence();
        damageEffectSeq.Append(_image.DOColor(Color.red, 0.15f));
        damageEffectSeq.Append(_image.DOColor(Color.white, 0.15f));
        damageEffectSeq.PlayForward();

    }
}
