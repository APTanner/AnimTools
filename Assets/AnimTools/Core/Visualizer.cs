using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimTools.ShapeData;
using Shapes;
using System.Drawing;

namespace AnimTools
{
    public sealed class Visualizer
    {
        private static Visualizer _instance;
        public static Visualizer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Visualizer();
                }
                return _instance;
            }
        }
        // so they can't be created
        private Visualizer() 
        {
        }

        #region Lines
        public LineStyle LineStyle { get; set; } = LineStyle.Default;
        public void DrawLine(Vector3 start, Vector3 end)
        {
            ShapeData.Line line = ShapeData.Line.Simple(start, end);
            line.LineStyle = LineStyle;
            VisualizationDrawer.Instance.Submit(line);
        }

        public void DrawLine(Vector3 start, Vector3 end, float p)
        {
            Vector3 dir = end - start;
            end = start + dir * p;
            DrawLine(start, end);
        }
        #endregion

        #region LineDerivatives
        public void DrawArrow(Vector3 start, Vector3 end, float arrowSize, float p)
        {
            arrowSize *= LineStyle.Width;
            Vector3 dir = end - start;
            Vector3 arrowTipPos = start + dir * p;
            float currentArrowSize = arrowSize * Mathf.Clamp01(p*9); // first 10% of the arrow's life the arrowhead grows to full size
            Vector3 n_dir = dir.normalized;
            Vector3 arrowPos = arrowTipPos + currentArrowSize * -n_dir; // arrowSize is the height (in meters) of the arrowhead at max size
                                                         // triangle's origin is the bottom middle of the triangle
                                                         // if we want the arrow's tip to be right at the end, we have to go
                                                         // back the height of the arrow to find its origin
            ShapeData.Triangle arrowHead = ShapeData.Triangle.Simple(arrowPos, n_dir, currentArrowSize);
            arrowHead.TriangleStyle = TriangleStyle.SingleColor(LineStyle.EndColor);
            VisualizationDrawer.Instance.Submit(arrowHead);
            DrawLine(start, arrowPos);
        }
        public void DrawArrow(Vector3 start, Vector3 end, float p)
        {
            DrawArrow(start, end, 3, p);
        }
        public void DrawArrow(Vector3 start, Vector3 end)
        {
            DrawArrow(start, end, 1, 1);
        }
        #endregion

        #region Discs
        public void DrawPoint(Vector3 pos, float radius, UnityEngine.Color color)
        {
            ShapeData.Disc disc = ShapeData.Disc.Simple(pos, radius);
            DiscColors colors = color;
            disc.DiscStyle.Colors = colors;
            VisualizationDrawer.Instance.Submit(disc);
        }
        public void DrawPoint(Vector3 pos, float radius)
        {
            ShapeData.Disc disc = ShapeData.Disc.Simple(pos, radius);
            VisualizationDrawer.Instance.Submit(disc);
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

