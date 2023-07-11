using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MergeHelper : MonoBehaviour
{
    [SerializeField] private Image helperImage;
    [SerializeField] private SlotSpawner slotSpawner;
    [SerializeField] private MergeSystem mergeSystem;
    [SerializeField] private float hintInterval = 6f;
    [SerializeField] private float animationDuration = 2f;
    private bool _isNeedHelp = true;
    private Tween _helperTween;

    private void OnValidate()
    {
       if (slotSpawner == null)
            slotSpawner = GetComponent<SlotSpawner>();
        if (mergeSystem == null)
            mergeSystem = GetComponent<MergeSystem>();
    }

    private void Awake()
    {
        mergeSystem.OnSlotCarryStateChanged += (slot, flag) =>
        {
            helperImage.enabled = false;
            _helperTween.Kill();
        };
    }

    private IEnumerator Start()
    {
        RectTransform helperRect = (RectTransform)helperImage.transform;
        helperRect.sizeDelta = slotSpawner.SlotDictionary[0].GetComponent<RectTransform>().sizeDelta;

        WaitForSeconds wait = new(hintInterval);

        while (_isNeedHelp)
        {
            yield return wait;
            if (TryFindMatch(out SlotsMatch match))
                ShowHint(match);
        }
    }

    private void ShowHint(SlotsMatch match)
    {
        if (_helperTween != null)
            _helperTween.Kill();

        helperImage.enabled = true;
        helperImage.sprite = match.Slot1.CurrentItem.ItemSprite;
        helperImage.transform.position = match.Slot1.transform.position;

        _helperTween = helperImage.transform.DOMove(match.Slot2.transform.position, animationDuration);
        _helperTween.OnComplete(() => helperImage.enabled = false);
    }

    private bool TryFindMatch(out SlotsMatch slotsMatch)
    {
        int slotsCount = slotSpawner.SlotDictionary.Count;
        for (int i = 0; i < slotsCount; i++)
        {
            Slot slot1 = slotSpawner.SlotDictionary[i];
            if (slot1.CurrentItem == null)
                continue;

            for (int j = i + 1; j < slotsCount; j++)
            {
                Slot slot2 = slotSpawner.SlotDictionary[j];
                if (slot2.CurrentItem == null)
                    continue;

                if (MergeSystem.TryMerge(slot1.CurrentItem, slot2.CurrentItem))
                {
                    slotsMatch = new(slot1, slot2);
                    return true;
                }
            }
        }
        slotsMatch = new();
        return false;
    }

    private struct SlotsMatch
    {
        public Slot Slot1;
        public Slot Slot2;

        public SlotsMatch(Slot slot1, Slot slot2)
        {
            Slot1 = slot1;
            Slot2 = slot2;
        }
    }
}
