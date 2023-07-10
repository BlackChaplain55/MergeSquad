using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSystem : MonoBehaviour
{
    [field: SerializeField] public Vector3 GroundPosition { get; private set; }
    [field: SerializeField] public float FireBallFallDuration { get; private set; }
    [field: SerializeField] public Vector3 FireBallSpawnPosition { get; private set; }
    [field: SerializeField] public ParticleSystem FireBallProjectile { get; private set; }
    [field: SerializeField] public ParticleSystem FireBallExplosion { get; private set; }

    public void CastMagic(ItemType type)
    {
        switch (type)
        {
            case ItemType.MagicFire:
                UseFireBall();
            break;

            default: break;
        }
    }

    [Button]
    private void UseFireBall()
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
                () => fireBall.localPosition = FireBallSpawnPosition);
        };
    }
}