using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimTools.ShapeData;
using Shapes;
using System.Drawing;

namespace AnimTools
{
    public static class Visualizer
    {
        public static ShapeStyle ShapeStyle { get; set; } = new ShapeStyle();

        #region Lines
        public static LineStyle LineStyle { get; set; } = new LineStyle();
        public static void DrawLine(Vector3 start, Vector3 end)
        {
            ShapeData.Line line = 
                new ShapeData.Line(start, end, new ShapeStyle(ShapeStyle), new LineStyle(LineStyle));
            VisualizationDrawer.Instance.AddShape(line);
        }

        public static void DrawLine(Vector3 start, Vector3 end, float p)
        {
            Vector3 dir = end - start;
            end = start + dir * p;
            DrawLine(start, end);
        }
        #endregion

        #region Triangle
        public static TriangleStyle TriangleStyle { get; set; } = new TriangleStyle();
        public static void DrawTriangle(Vector3 position, Vector3 heading, float size)
        {
            VisualizationDrawer.Instance.AddShape(
                new ShapeData.Triangle(position, heading, size, new ShapeStyle(ShapeStyle), new TriangleStyle(TriangleStyle))
            );
        }

        #endregion

        #region LineDerivatives
        public static void DrawArrow(Vector3 start, Vector3 end, float arrowSize, float p)
        {
            arrowSize *= ShapeStyle.Thickness;
            Vector3 dir = end - start;
            Vector3 arrowTipPos = start + dir * p;
            float currentArrowSize = arrowSize * Mathf.Clamp01(p*9); // first 10% of the arrow's life the arrowhead grows to full size
            Vector3 n_dir = dir.normalized;
            Vector3 arrowPos = arrowTipPos + currentArrowSize * -n_dir; // arrowSize is the height (in meters) of the arrowhead at max size
                                                                        // triangle's origin is the bottom middle of the triangle
                                                                        // if we want the arrow's tip to be right at the end, we have to go
                                                                        // back the height of the arrow to find its origin
            ShapeData.Triangle arrowHead =
                new ShapeData.Triangle(arrowPos, n_dir, currentArrowSize, new ShapeStyle(ShapeStyle), new TriangleStyle(TriangleStyle));
            
            if (LineStyle.EndColor != UnityEngine.Color.clear)
            {
                arrowHead.ShapeStyle.Color = LineStyle.EndColor;
            }

            VisualizationDrawer.Instance.AddShape(arrowHead);
            DrawLine(start, arrowPos);
        }
        public static void DrawArrow(Vector3 start, Vector3 end, float p)
        {
            DrawArrow(start, end, 3, p);
        }
        public static void DrawArrow(Vector3 start, Vector3 end)
        {
            DrawArrow(start, end, 1, 1);
        }
        #endregion

        #region Discs
        public static DiscStyle DiscStyle { get; set; } = new DiscStyle();
        public static void DrawPoint(Vector3 pos, float radius)
        {
            ShapeData.Disc disc =
                new ShapeData.Disc(pos, radius, -Camera.main.transform.forward, new ShapeStyle(ShapeStyle), new DiscStyle(DiscStyle));
            VisualizationDrawer.Instance.AddShape(disc);
        }
        #endregion

        #region Rectangles
        public static RectangleStyle RectangleStyle { get; set; } = new RectangleStyle();
        public static void DrawRectangle(Vector3 pos, Vector2 size, float p)
        {
            DrawRectangle(pos, size, Vector3.forward, p);
        }

        public static void DrawRectangle(Vector3 pos, Vector2 size, Vector3 normal, float p)
        {
            RectangleStyle rectStyle = new RectangleStyle(RectangleStyle);
            rectStyle.BorderThickness *= p;
            ShapeData.Rectangle rectangle =
                new ShapeData.Rectangle(pos, normal, size * p, new ShapeStyle(ShapeStyle), rectStyle);
            VisualizationDrawer.Instance.AddShape(rectangle);
        }

        #endregion
        //#region Graphs
        //public void Draw2DGraph(Vector3 origin, float extent, float gridSpacing, float p)
        //{
        //    float axisP = Mathf.Clamp01(2 * p);
        //    DrawArrow(origin, origin + new Vector3(extent, 0f, 0f), axisP);
        //    DrawArrow(origin, origin + new Vector3(0f, extent, 0f), axisP);
        //    for (int i = 1; i <= extent / gridSpacing; i++)
        //    {
        //        float distFromOrigin = i * gridSpacing;
        //        float pAlongLine = distFromOrigin / extent;
        //        float lineP = Mathf.Clamp01(2*p - pAlongLine);
        //        DrawLine(new Vector3(origin.x + distFromOrigin, origin.y, origin.z), new Vector3(origin.x + distFromOrigin, origin.y + extent, origin.z), lineP);
        //        DrawLine(new Vector3(origin.x, origin.y + distFromOrigin, origin.z), new Vector3(origin.x + extent, origin.y + distFromOrigin, origin.z), lineP);
        //    }
        //}
        //#endregion

    }
}

