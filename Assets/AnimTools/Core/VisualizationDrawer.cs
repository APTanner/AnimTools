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


        private List<IShape> m_shapes = new List<IShape>();
        private List<IDashedShape> m_dashedShapes = new List<IDashedShape>();

        public override void DrawShapes(Camera cam)
        {
            Draw.BlendMode = ShapesBlendMode.Opaque;
            using (Draw.Command(cam))
            {
                using (Draw.DashedScope())
                {
                    foreach (IDashedShape shape in m_dashedShapes)
                    {
                        ShapeStyle shapeStyle = shape.Style;
                        Draw.BlendMode = shapeStyle.BlendMode;
                        Draw.Color = shapeStyle.Color;
                        Draw.Thickness = shapeStyle.Thickness;
                        Draw.ThicknessSpace = shapeStyle.ThicknessSpace;

                        switch (shape.Type)
                        {
                            case DashedShapeType.DashedLine:
                                DashedLine line = (DashedLine)shape;
                                Draw.DashSize = line.DashStyle.DashSize;
                                Draw.DashSpacing = line.DashStyle.DashSpacing;
                                Draw.DashSpace = line.DashStyle.DashSpace;
                                Draw.DashType = line.DashStyle.DashType;
                                if (line.LineStyle.StartColor == Color.clear)
                                {
                                    Draw.Line(line.Start, line.End, line.LineStyle.LineEndCap);
                                }
                                else
                                {
                                    Draw.Line(line.Start, line.End, line.LineStyle.LineEndCap,
                                    line.LineStyle.StartColor, line.LineStyle.EndColor);
                                }
                                break;
                            case DashedShapeType.DashedDisc:
                                // TODO
                                break;
                            default:
                                throw new System.Exception("Invalid ShapeType");

                        }
                    }
                    m_dashedShapes.Clear();
                }
                
                foreach(IShape shape in m_shapes)
                {
                    ShapeStyle shapeStyle = shape.Style;
                    Draw.BlendMode = shapeStyle.BlendMode;
                    Draw.Color = shapeStyle.Color;
                    Draw.Thickness = shapeStyle.Thickness;
                    Draw.ThicknessSpace = shapeStyle.ThicknessSpace;

                    switch (shape.Type)
                    {
                        case ShapeType.Line:
                            ShapeData.Line line = (ShapeData.Line)shape;
                            if (line.LineStyle.StartColor == Color.clear)
                            {
                                Draw.Line(line.Start, line.End, line.LineStyle.LineEndCap);
                            }
                            else
                            {
                                Draw.Line(line.Start, line.End, line.LineStyle.LineEndCap,
                                line.LineStyle.StartColor, line.LineStyle.EndColor);
                            }
                            break;
                        case ShapeType.Disc:
                            ShapeData.Disc disc = (ShapeData.Disc)shape;
                            if (disc.DiscStyle.UsingColors)
                            {
                                Draw.Disc(disc.Position, disc.Normal, disc.Radius, disc.DiscStyle.Colors);
                            }
                            else
                            {
                                Draw.Disc(disc.Position, disc.Normal, disc.Radius);
                            }
                            break;
                        case ShapeType.Triangle:
                            ShapeData.Triangle triangle = (ShapeData.Triangle)shape;
                            var positions = triangle.GetWorldSpaceVertexPositions();
                            Draw.Triangle(
                                positions.A,
                                positions.B,
                                positions.C,
                                triangle.TriangleStyle.Roundness,
                                Color.white
                            );
                            break;
                        case ShapeType.Rectangle:
                            ShapeData.Rectangle rectangle = (ShapeData.Rectangle)shape;
                            Draw.Rectangle(
                                rectangle.Position,
                                rectangle.Normal,
                                rectangle.Size.x, 
                                rectangle.Size.y,
                                rectangle.RectangleStyle.Roundness,
                                rectangle.ShapeStyle.Color
                                );
                            if (rectangle.RectangleStyle.BorderThickness == 0) { break; }
                            Draw.RectangleBorder(
                                rectangle.Position, 
                                rectangle.Normal, 
                                rectangle.Size,
                                rectangle.RectangleStyle.BorderThickness,
                                rectangle.RectangleStyle.Roundness,
                                rectangle.RectangleStyle.BorderColor
                                );
                            break;
                        default:
                            throw new System.Exception("Invalid ShapeType");
                    }
                }
                m_shapes.Clear();
            }
        }

        public void AddShape(IShape shape)
        {
           m_shapes.Add(shape);
        }

        public void AddDashedShape(IDashedShape shape)
        {
            m_dashedShapes.Add(shape);
        }
    }
}

