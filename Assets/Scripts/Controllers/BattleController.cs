using UnityEngine;
using System.Collections.Generic;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;
    public PlayerColor CurrentPlayer { get; private set; } = PlayerColor.White;
    public Unit selectedUnit;

    private Battlefield battlefield;
    private List<Vector2Int> availableMoves = new List<Vector2Int>();

    private void Awake()
    {
        Instance = this;
        battlefield = FindObjectOfType<Battlefield>();
    }

    // Публичный метод для подсветки доступных ходов
    public void HighlightAvailableMoves(Unit unit)
    {
        availableMoves.Clear();
        Vector2Int pos = unit.CurrentCell.GridPosition;

        // Логика определения доступных ходов (пример для шашки)
        int direction = unit.Color == PlayerColor.White ? 1 : -1;
        Vector2Int[] possibleMoves = {
            new Vector2Int(pos.x - 1, pos.y + direction),
            new Vector2Int(pos.x + 1, pos.y + direction)
        };

        foreach (Vector2Int move in possibleMoves)
        {
            if (move.x >= 0 && move.x < 8 && move.y >= 0 && move.y < 8)
            {
                Cell cell = battlefield.GetCell(move);
                if (cell != null && cell.OccupyingUnit == null)
                {
                    availableMoves.Add(move);
                    cell.highlightRenderer.color = Color.yellow;
                }
            }
        }
    }

    // Публичный метод проверки валидности хода
    public bool IsValidMove(Unit unit, Cell targetCell)
    {
        if (unit == null || targetCell == null) return false;
        Vector2Int from = unit.CurrentCell.GridPosition;
        Vector2Int to = targetCell.GridPosition;

        // Проверка, занята ли клетка
        if (targetCell.OccupyingUnit != null) return false;

        // Проверка границ доски
        if (to.x < 0 || to.x >= 8 || to.y < 0 || to.y >= 8) return false;

        if (unit.Type == UnitType.Checker)
        {
            int direction = unit.Color == PlayerColor.White ? 1 : -1;
            if (to.y - from.y != direction) return false;
            if (Mathf.Abs(to.x - from.x) != 1) return false;
        }
        else // King
        {
            if (Mathf.Abs(to.x - from.x) != Mathf.Abs(to.y - from.y)) return false;
        }

        return true;
    }

    // Публичный метод выполнения хода
    public void ExecuteMove(Unit unit, Cell targetCell)
    {
        // Убираем с предыдущей клетки
        unit.CurrentCell.SetUnit(null);

        // Ставим на новую клетку
        targetCell.SetUnit(unit);
        unit.CurrentCell = targetCell;

        // Сбрасываем выбор
        selectedUnit = null;

        // Смена хода
        CurrentPlayer = CurrentPlayer == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
    }

    public void OnCellClicked(Cell cell)
    {
        if (selectedUnit == null)
        {
            if (cell.OccupyingUnit != null &&
                cell.OccupyingUnit.Color == CurrentPlayer)
            {
                selectedUnit = cell.OccupyingUnit;
                HighlightAvailableMoves(selectedUnit);
            }
        }
        else
        {
            if (IsValidMove(selectedUnit, cell))
            {
                ExecuteMove(selectedUnit, cell);
            }
            else
            {
                Debug.Log("Неверный ход!");
            }
        }
    }
}
