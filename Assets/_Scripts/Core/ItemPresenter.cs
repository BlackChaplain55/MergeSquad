using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPresenter : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI typeText;
    private Shadow _shadow;

    private void OnValidate()
    {
        icon ??= GetComponent<Image>();
        _shadow ??= GetComponent<Shadow>();
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
        levelText.text = (id + 1).ToString();
        typeText.text = MergeData.GetShortName(type);
    }

    public void SetItem(ItemSO item)
    {
        SetColor(Color.white);
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

    public void SetColor(Color value)
    {
        icon.color = value;
    }

    public void SetShadowColor(Color value)
    {
        if (_shadow == null) return;
        _shadow.effectColor = value;
    }

    public void SetIcon(Sprite sprite)
    {
        if (icon == null) return;
        
        icon.enabled = sprite != null;
        icon.sprite = sprite;
    }

    public Color GetColor() => icon.color;
    public Sprite GetSprite() => icon.sprite;

    public void Clear()
    {
        levelText?.SetText(string.Empty);
        typeText?.SetText(string.Empty);
        SetIcon(null);
    }
}
