using UnityEngine;

public class CheckersUnit : Unit
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer не найден на объекте!");
        }
    }

    public override void SetType(UnitType newType)
    {
        base.SetType(newType);
        if (newType == UnitType.King)
        {
            // Явно указываем пространство имён UnityEngine.Color
            spriteRenderer.color = UnityEngine.Color.red;
        }
    }
}
