using UnityEngine;

public class Battlefield : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;

    private Cell[,] cells;

    private void Start()
    {
        CreateBoard();
        SpawnInitialUnits();
    }

    private void CreateBoard()
    {
        cells = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cellObj = Instantiate(cellPrefab.gameObject);
                cellObj.transform.position = new Vector3(x, y, 0);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.GridPosition = new Vector2Int(x, y);
                cells[x, y] = cell;
            }
        }
    }

    // Публичный метод для получения клетки по координатам
    public Cell GetCell(Vector2Int position)
    {
        if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height)
            return null;
        return cells[position.x, position.y];
    }

    private void SpawnInitialUnits()
    {
        // Логика расстановки начальных юнитов
    }
}
