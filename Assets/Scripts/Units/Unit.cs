using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public PlayerColor Color { get; protected set; }
    public UnitType Type { get; protected set; }
    public Cell CurrentCell { get; set; }

    public void SetColor(PlayerColor color)
    {
        Color = color;
    }

    public virtual void SetType(UnitType newType)
    {
        Type = newType;
    }
}
