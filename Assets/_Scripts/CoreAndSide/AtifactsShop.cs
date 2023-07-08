using System.Collections.Generic;
using UnityEngine;

public class AtifactsShop : MonoBehaviour
{
    [SerializeField] private GameObject ShopObject;
    private bool _isVisible;

    public void ToggleVisibility()
    {
        _isVisible = !_isVisible;
        ShopObject.SetActive(_isVisible);
    }
}