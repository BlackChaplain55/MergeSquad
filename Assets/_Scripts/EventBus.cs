using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventBus
{
    public static Action<int> onSceneChange;
    public static Action<Unit> onUnitDeath;
    public static Action<Unit> onBossDeath;
    public static Action onHeroDeath;
    public static Action onFinalBossDeath;
    public static Action<Vector2> onHeroMove;
}