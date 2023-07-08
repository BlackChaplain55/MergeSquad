using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField] private List<Parallax> _layers;
    // Start is called before the first frame update
    private void Awake()
    {
        EventBus.onHeroMove += MoveParallax;
    }

    private void OnDestroy()
    {
        EventBus.onHeroMove -= MoveParallax;
    }

    private void MoveParallax(Vector2 position)
    {
        foreach(Parallax layer in _layers)
        {
            layer.SetPosition(position);
        }
    }
}
