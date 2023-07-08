using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parallax : MonoBehaviour
{
    [field: SerializeField] public Vector2 Position { get; private set; }
    [field: SerializeField] public Vector2 Offset { get; private set; }
    [field: SerializeField] public Vector2 RelativeSpeed { get; private set; }
    [field: SerializeField] public Vector2 UnitScale { get; private set; }
    [SerializeField] private Transform child1;
    [SerializeField] private Transform child2;
    private float _currentChild = 0;
    private float _width;
    private float _verticalPosition;

    private void Awake()
    {
        EventBus.onHeroMove += SetPosition;
        _width = child1.GetComponent<Image>().rectTransform.sizeDelta.x;
        _verticalPosition = child1.GetComponent<Image>().rectTransform.sizeDelta.y;
    }

    private void OnDisable()
    {
        EventBus.onHeroMove -= SetPosition;
    }
    private void OnValidate()
    {
        if (child1 == null)
            child1 = transform.GetChild(0);
        if (child2 == null)
            child2 = transform.GetChild(1);
    }

    public void SetPosition(Vector2 value)
    {
        //Position = value;
        //if (Position.x == _width)
        //{
        //    UpdateRect();
        //}
        
    }

    [ExecuteInEditMode()]
    public void UpdateRect()
    {
        if (_currentChild == 0)
        {
            child1.transform.localPosition = new Vector2(child1.transform.localPosition.x + 2 * _width, _verticalPosition);
            _currentChild = 1;
        }
        else
        {
            _currentChild = 0;
        }
        //Vector2 relativeOffset = Position * RelativeSpeed;
        //var relativeX = Mathf.Repeat(relativeOffset.x, UnitScale.x);
        //int intX = Mathf.FloorToInt((Position.x + relativeX) / UnitScale.x);
        //int intY = Mathf.FloorToInt(Position.y / UnitScale.y);
        //float x = intX * UnitScale.x - relativeX;
        //float y = 0;
        //float x2 = (intX + 1) * UnitScale.x - relativeX;// Mathf.Repeat(Position.x * RelativeSpeed.x, UnitScale.x);
        //child1.localPosition = new Vector2(x, y) + Offset;
        //child2.localPosition = new Vector2(x2, y) + Offset;
    }


    /// <summary>
    /// Repeat value between -max and max
    /// </summary>
    private float Repeat(float value, float max)
    {
        float t = (value - max / 2) / max;
        return (t - Mathf.Floor(t) - 0.5f) * max;
    }
}
