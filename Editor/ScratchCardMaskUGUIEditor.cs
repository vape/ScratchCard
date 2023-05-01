using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ScratchCard.Editor
{
    [CustomEditor(typeof(ScratchCardMaskUGUI))]
    public class ScratchCardMaskUGUIEditor : UnityEditor.Editor
    {
        private const float GizmoAlpha = 0.15f;
        
        [DrawGizmo(GizmoType.InSelectionHierarchy)]
        public static void DrawGizmos(ScratchCardMaskUGUI mask, GizmoType type)
        {
            if (mask.Grid.Valid)
            {
                DrawGridGizmos(mask.Image, mask.Grid, GizmoAlpha);
            }
        }
        
        private static void DrawGridGizmos(Graphic graphic, ScratchCardGrid grid, float alpha)
        {
            var rect = graphic.rectTransform.rect;
            var savedMatrix = Gizmos.matrix;
            var savedColor = Gizmos.color;

            Gizmos.matrix = graphic.transform.localToWorldMatrix;

            var mouseOverCell = grid.LocalPointToCell(rect, ScratchCardMaskUGUI.ScreenToGraphicLocalPoint(graphic,Input.mousePosition));

            for (int x = 0; x < grid.Width; ++x)
            {
                for (int y = 0; y <  grid.Height; ++y)
                {
                    var cellRect = grid.CalculateCellRect(rect, x, y);
                    var cellRectSize = cellRect.size - Vector2.one; // make a small gap between cells

                    var visited = grid.GetVisited(x, y);
                    var mouseOver = x == mouseOverCell.x && y == mouseOverCell.y;
                    var color = mouseOver ? Color.yellow : visited ? Color.green : Color.red;
                    color.a *= alpha;
                    
                    Gizmos.color = color;
                    Gizmos.DrawCube(cellRect.position, cellRectSize);
                }
            }

            Gizmos.matrix = savedMatrix;
            Gizmos.color = savedColor;
        }
    }
}
