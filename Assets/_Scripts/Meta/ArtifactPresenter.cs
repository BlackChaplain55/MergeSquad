using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactPresenter : MonoBehaviour
{
    [SerializeField] private Artifact artifact;
    [SerializeField] private int cost;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    private void OnValidate()
    {
        if (icon == null)
            icon = GetComponent<Image>();
        if (buyButton == null)
            buyButton = GetComponent<Button>();
    }

    public void Init(Artifact artifact)
    {
        this.artifact = artifact;
        cost = 55;
        artifact.PropertyChanged += UpdateText;
        InitializePresenter();
    }

    private void InitializePresenter()
    {
        var data = artifact.BaseData;
        icon.sprite = data.Icon;
        _name.SetText(data.ArtifactName);
        count.SetText($"{artifact.Count}/{artifact.BaseData.MaxCount}");
        description.SetText(string.Format(
            data.ArtifactDescription,
            data.MultiplierPerPiece,
            data.MultiplierPerPiece * artifact.Count));
        costText.SetText(cost.ToString());
        buyButton.onClick.AddListener(() =>
        {
            if (GameController.Game.TrySpendCrystals(cost))
                artifact.SetCount(artifact.Count + 1);
        });
    }

    private void UpdateText(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Artifact.Count))
        {
            count.SetText($"{artifact.Count}/{artifact.BaseData.MaxCount}");
            var data = artifact.BaseData;
            description.SetText(string.Format(
                data.ArtifactDescription,
                data.MultiplierPerPiece,
                data.MultiplierPerPiece * artifact.Count));
        }
    }
}