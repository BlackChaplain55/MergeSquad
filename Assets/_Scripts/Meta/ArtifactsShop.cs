using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactsShop : MonoBehaviour
{
    [SerializeField] private GameObject artifactPresenterTemplate;
    [SerializeField] private Transform artifactPresenterParent;
    [SerializeField] private List<ArtifactPresenter> ArtifactsSlots;
    [SerializeField] private GameObject ShopObject;
    private bool _isVisible;

    public void ToggleVisibility()
    {
        _isVisible = !_isVisible;
        ShopObject.SetActive(_isVisible);
    }

    private void Start()
    {
        var artifactsRepository = GameController.Game.ArtifactsRepository;
        foreach (var type in artifactsRepository.UnitArtifactsData)
        {
            foreach (var artifact in type.Value)
            {
                var artifactGO = Instantiate(artifactPresenterTemplate, artifactPresenterParent);
                ArtifactPresenter presenter = artifactGO.GetComponent<ArtifactPresenter>();

                presenter.Init(artifact);
                ArtifactsSlots.Add(presenter);
            }
        }
    }
}
