using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MagicSystem : MonoBehaviour
{
    [field: SerializeField] public Vector3 GroundPosition { get; private set; }
    [field: SerializeField] public float FireBallFallDuration { get; private set; }
    [field: SerializeField] public Vector3 FireBallSpawnPosition { get; private set; }
    [field: SerializeField] public ParticleSystem FireBallProjectile { get; private set; }
    [field: SerializeField] public ParticleSystem FireBallExplosion { get; private set; }
    [SerializeField] private UnitController unitController;

    public void CastMagic(Hero hero, MagicSO magic)
    {
        switch (magic.Type)
        {
            case ItemType.MagicFire:
                UseFireBall(hero, magic);
            break;

            default: break;
        }
    }

    private void UseFireBall(Hero hero, MagicSO magic)
    {
        Transform fireBall = FireBallProjectile.transform;
        fireBall.localPosition = FireBallSpawnPosition;
        var fireBallImage = FireBallProjectile.GetComponent<Image>();
        FireBallProjectile.Play();
        fireBallImage.color = Color.white;

        //Vector3 direction = (GroundPosition - fireBall.localPosition) / FireBallFallDuration;
        //fireBall.rotation.eulerAngles = 
         
        //var emitter = FireBallProjectile.main;
        //emitter.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;

        var fireBallFall = fireBall
            .DOLocalMove(
            new Vector3(GroundPosition.x, GroundPosition.y, fireBall.localPosition.z),
            FireBallFallDuration)
            .SetEase(Ease.Linear);

        fireBallFall.onComplete =
        () =>
        {
            fireBallImage.color = Color.clear;
            FireBallProjectile.Stop();
            FireBallExplosion.Play();
            DOVirtual.DelayedCall(
                FireBallExplosion.main.duration,
                OnFireballGrounded);
        };

        void OnFireballGrounded()
        {
            fireBall.localPosition = FireBallSpawnPosition;
            float minRange = GroundPosition.x - magic.BaseRange;
            float maxRange = GroundPosition.x + magic.BaseRange;
            Func<Unit, bool> isEnemyInRange = unit => unit.isEnemy && unit.Position > minRange && unit.Position < maxRange;
            var enemies = (List<Unit>)unitController.UnitsList.Where(isEnemyInRange);
            
            enemies.ForEach(enemy => enemy.TakeDamage(hero.MagicStrength));
        };
    }
}