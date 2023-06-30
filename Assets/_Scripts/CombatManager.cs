using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [field: SerializeField] public int linesCount;
    [field: SerializeField] public float linesSpacing;
    [field: SerializeField] public float walkSpeedMultiplier;
    public static CombatManager Combat;
    
    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
