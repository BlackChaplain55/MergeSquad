using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [field: SerializeField] public int linesCount;
    [field: SerializeField] public float linesSpacing;
    [field: SerializeField] public float walkSpeedMultiplier;
    [field: SerializeField] public float walkSpeedDiviation;
    [field: SerializeField] public float AttackDistanceDiviation;
    public static CombatManager Combat;
    public bool IsGame;
    
    private void Awake()
    {
        if (Combat == null)
        {
            DontDestroyOnLoad(gameObject);
            Combat = this;
        }
        else if (Combat != this)
        {
            Destroy(gameObject);
        }

        IsGame = true;
    }
}
