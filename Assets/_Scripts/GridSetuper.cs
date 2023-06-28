using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridSetuper : MonoBehaviour
{
    [SerializeField] private float minSpacing;
    [SerializeField] private Vector2 fieldSize;
    private RectTransform _thisRect;
    private GridLayoutGroup _grid;

    [ContextMenu(nameof(SetupGrid))]
    public void SetupGrid()
    {
        float parentWidth = _thisRect.rect.width - _grid.padding.left - _grid.padding.right - minSpacing * fieldSize.x;
        float parentHeight = _thisRect.rect.height - _grid.padding.top - _grid.padding.bottom - minSpacing * fieldSize.y;
        float width = parentWidth / fieldSize.x;
        float height = parentHeight / fieldSize.y;
        if (width < height)
        {
            float delta = height - width;
            _grid.cellSize = new Vector2(width, width);
            _grid.spacing = new Vector2(minSpacing, delta);
            _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _grid.constraintCount = (int)fieldSize.x;
        }
        else
        {
            float delta = width - height;
            _grid.cellSize = new Vector2(height, height);
            _grid.spacing = new Vector2(delta, minSpacing);
            _grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            _grid.constraintCount = (int)fieldSize.y;
        }
    }    

    private void OnValidate() => Validate();
    private void Start() => Validate();
    private void Validate()
    {
        if (_grid == null)
            _grid = GetComponent<GridLayoutGroup>();
        if (_thisRect == null)
            _thisRect = GetComponent<RectTransform>();

        SetupGrid();
    }
}