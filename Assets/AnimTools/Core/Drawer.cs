using AnimTools;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Drawer : ImmediateModeShapeDrawer
{
    private PriorityQueue<Shape> _pq = new PriorityQueue<Shape>();

    public override void DrawShapes(Camera cam)
    {
        if (_pq.Count == 0) return;

        using (Draw.Command(cam))
        {
            // handle dashed elements
            using (Draw.DashedScope())
            {
                Shape shape = _pq.Dequeue();
                // enum is less than or equal to the last dashed object (therefore dashed object)
                while (_pq.Count >= 0 && (int)shape.ShapeType <= (int)ShapeType.DashedDisc)
                {
                    switch (shape.ShapeType)
                    {
                    case ShapeType.DashedLine:
                        DashedLine line = shape as DashedLine;
                        Draw.DashSize = line.DashData.DashSize;
                        Draw.DashSpacing = line.DashData.DashSpacing;
                        Draw.DashSpace = line.DashData.DashSpace;
                        Draw.DashType = line.DashData.DashType;
                        Draw.Line(line.Start, line.End, line.Thickness, line.EndCaps, line.ColorStart, line.ColorEnd);
                        break;
                    case ShapeType.DashedDisc:
                        // TODO
                        break;
                    default:
                        throw new System.Exception("Invalid ShapeType");
                    }
                    if (_pq.Count == 0) break;
                    shape = _pq.Dequeue();
                }
            }
            while (_pq.Count > 0)
            {
                Shape shape = _pq.Dequeue();
                switch (shape.ShapeType)
                {
                    case ShapeType.Line:
                        Line line = shape as Line;
                        Draw.Line(line.Start, line.End, line.Thickness, line.EndCaps, line.ColorStart, line.ColorEnd);
                        break;
                    case ShapeType.Disc:
                        Disc disc = shape as Disc;
                        break;
                    default:
                        throw new System.Exception("Invalid ShapeType");
                }
            }
        }
    }

    public void Submit(Shape shape)
    {
        _pq.Enqueue(shape);
    }
}
