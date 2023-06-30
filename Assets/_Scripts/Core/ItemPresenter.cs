using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPresenter : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI typeText;

    private void OnValidate()
    {
        icon ??= GetComponent<Image>();
        if (levelText == null || typeText == null)
        {
            TextMeshProUGUI[] texts = GetComponents<TextMeshProUGUI>();
            //levelText ??= texts[0];
            //typeText ??= texts[1];
        }
    }

    public void Init(int id, ItemType type)
    {
        icon.sprite = MergeData.GetItemVisualById(type, id);
        levelText.text = id.ToString();
        typeText.text = MergeData.GetShortName(type);
    }

    public void SetItem(ItemSO item)
    {
        SetLevel(item.Id);
        SetType(item.Type);
        SetIcon(MergeData.GetItemVisualById(item.Type, item.Id));
    }

    public void SetLevel(int level)
    {
        if (levelText == null) return;

        levelText.text = (++level).ToString();
    }

    public void SetType(ItemType type)
    {
        if (typeText == null) return;

        typeText.text = MergeData.GetShortName(type);
    }

    public void SetIcon(Sprite sprite)
    {
        if (icon == null) return;
        
        icon.enabled = sprite != null;
        icon.sprite = sprite;
    }

    public void Clear()
    {
        levelText?.SetText(string.Empty);
        typeText?.SetText(string.Empty);
        SetIcon(null);
    }
}
