using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpawnEffects : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spawnCircles;
    [SerializeField] private GameObject _spawnCircleTemplate;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _enemyOffsetX;
    [SerializeField] private float _offsetY;


    public void SpawnEffect(Transform parent, bool isEnemy)
    {
        foreach(GameObject currentCircle in _spawnCircles)
        {
            if (!currentCircle.activeSelf)
            {
                currentCircle.SetActive(true);
                BlinkSpawnCirle(parent,currentCircle, isEnemy);
                return;
            }
        }
        GameObject newCircle = Instantiate(_spawnCircleTemplate, parent);
        _spawnCircles.Add(newCircle);
        BlinkSpawnCirle(parent, newCircle, isEnemy);
    }

    private void BlinkSpawnCirle(Transform parent, GameObject currentCircle,bool isEnemy)
    {
        currentCircle.transform.SetParent(parent);
        currentCircle.transform.SetAsFirstSibling();
        currentCircle.transform.position = parent.position;
        currentCircle.transform.Translate(_offsetX* currentCircle.transform.lossyScale.x,_offsetY* currentCircle.transform.lossyScale.y, 0);
        //Debug.Log("Spawn point: " + parent.gameObject.name + " at " + parent.position + " circle at " + currentCircle.transform.position);
        Image circleImage = currentCircle.GetComponent<Image>();
        var effectSeq = DOTween.Sequence();
        effectSeq.Append(circleImage.DOFade(1, 0.5f));
        effectSeq.Append(circleImage.DOFade(0, 1.5f).OnComplete(() => { currentCircle.SetActive(false); }));
        effectSeq.PlayForward();
    }
}
