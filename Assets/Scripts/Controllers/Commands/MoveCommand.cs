using UnityEngine;

public class MoveCommand : IGameplayCommand
{
    private BattleController battleController;
    private PlayerController playerController;

    public MoveCommand(BattleController controller, PlayerController playerCtrl)
    {
        battleController = controller;
        playerController = playerCtrl;
    }

    public void Interact(Cell cell)
    {
        if (battleController.selectedUnit == null)
        {
            if (cell.OccupyingUnit != null &&
                cell.OccupyingUnit.Color == battleController.CurrentPlayer)
            {
                battleController.selectedUnit = cell.OccupyingUnit;
                battleController.HighlightAvailableMoves(battleController.selectedUnit);
            }
        }
        else
        {
            if (battleController.IsValidMove(battleController.selectedUnit, cell))
            {
                playerController.VisualizeMove(
                    battleController.selectedUnit,
            battleController.selectedUnit.CurrentCell,
            cell);
                battleController.ExecuteMove(battleController.selectedUnit, cell);
            }
            else
            {
                Debug.Log("Неверный ход!");
            }
        }
    }
}
