using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] public SpriteRenderer highlightRenderer;
    public Vector2Int GridPosition { get; set; }
    public Unit OccupyingUnit { get; private set; }

    private Color originalColor;

    private void Awake()
    {
        if (highlightRenderer != null)
            originalColor = highlightRenderer.color;
    }

    /// <summary>
    /// Устанавливает юнита на клетку
    /// </summary>
    public void SetUnit(Unit unit)
    {
        OccupyingUnit = unit;
        if (unit != null)
            unit.transform.position = transform.position;
    }

    /// <summary>
    /// Обработчик наведения курсора на клетку
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightRenderer != null)
            highlightRenderer.color = Color.yellow;
    }

    /// <summary>
    /// Обработчик ухода курсора с клетки
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightRenderer != null)
            highlightRenderer.color = originalColor;
    }

    /// <summary>
    /// Обработчик клика по клетке
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        BattleController.Instance.OnCellClicked(this);
    }
}
