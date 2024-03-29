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
    [field: SerializeField] public RectTransform FireBall { get; private set; }
    [field: SerializeField] public ParticleSystem FireBallTrail { get; private set; }
    [field: SerializeField] public ParticleSystem FireBallExplosion { get; private set; }
    [field: SerializeField] public ParticleSystem FireBallExplosionFlipbook { get; private set; }
    [SerializeField] private UnitController unitController;
    [SerializeField] private Transform canvas;

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
        Transform fireBall = FireBall.transform;
        fireBall.localPosition = FireBallSpawnPosition;
        var fireBallImage = FireBall.GetComponent<Image>();
        FireBallTrail.Play();
        fireBallImage.color = Color.white;

        Vector3 direction = (GroundPosition - fireBall.localPosition) / FireBallFallDuration;
        float z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireBall.rotation = Quaternion.Euler(new Vector3(0, 0, z));

        //var emitter = FireBallTrail.main;
        //emitter.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;

        var fireBallFall = fireBall
            .DOLocalMove(
            new Vector3(GroundPosition.x, GroundPosition.y, fireBall.localPosition.z),
            FireBallFallDuration);
        fireBallFall.SetEase(Ease.Linear);

        fireBallFall.onComplete =
        () =>
        {
            fireBallImage.color = Color.clear;
            FireBallTrail.Stop();
            FireBallExplosion.Play(false);
            FireBallExplosionFlipbook.Play(false);
            OnFireballGrounded();
            var expl = FireBallExplosion.main;
            DOVirtual.DelayedCall(expl.duration, () => fireBall.localPosition = FireBallSpawnPosition);
        };

        void OnFireballGrounded()
        {
            float center = GroundPosition.x * canvas.localScale.x;
            float range = magic.BaseRange / 100;
            float minRange = center - range;
            float maxRange = center + range;
            Func<Unit, bool> isEnemyInRange = unit =>
            {
                bool result = unit.isEnemy && unit.Position > minRange && unit.Position < maxRange;
                Debug.Log($"unit.isEnemy {unit.isEnemy} && unit.Position {unit.Position} > minRange {minRange} && unit.Position {unit.Position} < maxRange {maxRange}");
                return result;
            };
            var enemies = unitController.EnemyList.Where(isEnemyInRange).ToList();
            enemies.ForEach(enemy =>
            {
                enemy.TakeDamage(hero.MagicStrength);
                });
            hero.SetMagic(null);
        };
    }
}