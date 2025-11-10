using UnityEngine.UI;

public class InvisibleGraphic : Graphic
{
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear(); // ничего не рисуем
    }
}