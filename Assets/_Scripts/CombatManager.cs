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
    [field: SerializeField] public float WalkBeginDelay;
    [field: SerializeField] public float CombatSFXVolume;
    [SerializeField] private int _damageTextPoolSize;
    [SerializeField] private GameObject _damageTextPrefabAlly;
    [SerializeField] private List<GameObject> _damageTextPoolAlly;
    [SerializeField] private GameObject _damageTextPrefabEnemy;
    [SerializeField] private List<GameObject> _damageTextPoolEnemy;

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

        for(int i=0; i < _damageTextPoolSize;i++)
        {
            var newDamageText = Instantiate(_damageTextPrefabAlly, transform);
            var newDamageTextEnemy = Instantiate(_damageTextPrefabEnemy, transform);
            newDamageText.SetActive(false);
            newDamageTextEnemy.SetActive(false);
            _damageTextPoolAlly.Add(newDamageText);
            _damageTextPoolEnemy.Add(newDamageTextEnemy);
        }
    }

    public GameObject GetDamageText(bool isEnemy)
    {
        GameObject damageTextObject = null;
        var currentPool = _damageTextPoolAlly;
        if (isEnemy) currentPool = _damageTextPoolEnemy;

        foreach(GameObject currentDamageText in currentPool)
        {
            if (!currentDamageText.activeSelf)
            {
                damageTextObject = currentDamageText;
                break;
            }
        }
        if (damageTextObject == null)
        {
            var currentPrefab = _damageTextPrefabAlly;
            if (isEnemy) currentPrefab = _damageTextPrefabEnemy;
            var newDamageText = Instantiate(currentPrefab, transform);
            newDamageText.SetActive(false);
            currentPool.Add(newDamageText);
            damageTextObject = newDamageText;
        }

        return damageTextObject;
    }
}
