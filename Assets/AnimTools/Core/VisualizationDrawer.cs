using AnimTools;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimTools.ShapeData;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;

namespace AnimTools
{
    [ExecuteAlways]
    public sealed class VisualizationDrawer : ImmediateModeShapeDrawer
    {
        private static VisualizationDrawer _instance;
        public static VisualizationDrawer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<VisualizationDrawer>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("Visualization Drawer");
                        _instance = obj.AddComponent<VisualizationDrawer>();
                        // Adding the component calls the Awake method on the VisualizationDrawer
                        // because it hasn't yet been assigned to _instance, this means it immediatly destroys itself
                        // handled by setting the instance to itself if the _instance field is null
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                DestroyImmediate(gameObject);
            }
        }


        private PriorityQueue<IShape> _dashedPq = new PriorityQueue<IShape>();
        private PriorityQueue<IShape> _normalPq = new PriorityQueue<IShape>();

        public override void DrawShapes(Camera cam)
        {
            Draw.BlendMode = ShapesBlendMode.Opaque;
            using (Draw.Command(cam))
            {
                using (Draw.DashedScope())
                {
                    // enum is less than or equal to the last dashed object (therefore the current object is dashed)
                    while (_dashedPq.Count > 0)
                    {
                        IShape shape = _dashedPq.Dequeue();
                        switch (shape.Type)
                        {
                            case ShapeType.DashedLine:
                                DashedLine line = (DashedLine)shape;
                                Draw.DashSize = line.DashStyle.DashSize;
                                Draw.DashSpacing = line.DashStyle.DashSpacing;
                                Draw.DashSpace = line.DashStyle.DashSpace;
                                Draw.DashType = line.DashStyle.DashType;
                                Draw.Line(line.Start, line.End, 
                                    line.LineStyle.Width, line.LineStyle.LineEndCap, 
                                    line.LineStyle.StartColor, line.LineStyle.EndColor);
                                break;
                            case ShapeType.DashedDisc:
                                // TODO
                                break;
                            default:
                                throw new System.Exception("Invalid ShapeType");
                        }
                    }
                }
                while (_normalPq.Count > 0)
                {
                    IShape shape = _normalPq.Dequeue();
                    switch (shape.Type)
                    {
                        case ShapeType.Line:
                            ShapeData.Line line = (ShapeData.Line)shape;
                            Draw.Line(line.Start, line.End,
                                line.LineStyle.Width, line.LineStyle.LineEndCap,
                                line.LineStyle.StartColor, line.LineStyle.EndColor);
                            break;
                        case ShapeType.Disc:
                            ShapeData.Disc disc = (ShapeData.Disc)shape;
                            Draw.Disc(disc.Position, disc.Normal, disc.Radius, disc.DiscStyle.Colors);
                            break;
                        case ShapeType.Triangle:
                            ShapeData.Triangle triangle = (ShapeData.Triangle)shape;
                            var positions = triangle.GetWorldSpaceVertexPositions();
                            Draw.Triangle(
                                positions.A,
                                positions.B,
                                positions.C,
                                triangle.TriangleStyle.Roundness,
                                triangle.TriangleStyle.Color
                            );
                            break;
                        default:
                            throw new System.Exception("Invalid ShapeType");
                    }
                }
            }
        }

        public void Submit(IShape shape)
        {
            if (shape.Type <= ShapeInfo.LastDashedShape)
            {
                _dashedPq.Enqueue(shape);
            }
            else
            {
                _normalPq.Enqueue(shape);
            }
        }
    }
}

