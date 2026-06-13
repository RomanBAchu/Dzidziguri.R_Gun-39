using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private MoveCommand moveCommand;

    private GameInput inputActions;

    private void Awake()
    {
        inputActions = new GameInput();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Gameplay.Select.performed += ctx => OnSelect();
        inputActions.Gameplay.Cancel.performed += ctx => OnCancel();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell != null)
            {
                moveCommand.Interact(cell);
            }
        }
    }

    private void OnCancel()
    {
        BattleController.Instance.selectedUnit = null;
        Debug.Log("Ход сброшен");
    }
}
