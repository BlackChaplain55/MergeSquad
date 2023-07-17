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
    [SerializeField] private List<Transform> _childs;
    [SerializeField] private Canvas _currentCanvas;

    private int _levelStep = 0;
    private int _currentChild = 0;
    private float _width;
    private float _levelLength;
    private float _verticalPosition;

    private void Awake()
    {
        EventBus.onHeroMove += SetPosition;
        EventBus.OnNextStepReached += ShiftBackground;
        if (_currentCanvas == null) _currentCanvas = transform.parent.parent.GetComponent<Canvas>();
        _width = _childs[0].GetComponent<Image>().rectTransform.sizeDelta.x;
        _levelLength = _currentCanvas.renderingDisplaySize.x;
    }

    private void OnDisable()
    {
        EventBus.onHeroMove -= SetPosition;
    }

    private void OnDestroy()
    {
        EventBus.OnNextStepReached -= ShiftBackground;
    }

    private void OnValidate()
    {
        if (_childs.Count == 0)
        {
            _childs.Add(transform.GetChild(0));
        }
    }

    public void SetPosition(Vector2 value)
    {
        Position = value;
        UpdateRect();
    }

    private void UpdateRect()
    {
        if (RelativeSpeed.x>0)
        {
            transform.Translate(new Vector3(-RelativeSpeed.x, 0, 0));
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

    private void ShiftBackground()
    {
        _childs[_currentChild].localPosition = new Vector3(_childs[_currentChild].localPosition.x+_width * _childs.Count, _childs[_currentChild].localPosition.y, 0);
        _currentChild++;
        if (_currentChild == _childs.Count) _currentChild = 0;
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
