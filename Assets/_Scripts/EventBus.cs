using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventBus
{
    public static Action<int> onSceneChange;
    public static Action<Unit> OnUnitDeath;
    public static Action OnBossDeath;
    public static Action OnHeroDeath;
    public static Action OnFinalBossDeath;
    public static Action<Vector2> onHeroMove;
}