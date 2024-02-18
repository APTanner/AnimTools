using JetBrains.Annotations;
using Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace AnimTools.ShapeData
{
    public enum ShapeType
    {
        None = 0,
        Line,
        Polyline,
        Disc,
        Rectangle,
        Quad,
        Triangle,
        RegularPolygon,
        Sphere,
        Torus,
        Cone,
        Cuboid
    }

    public enum DashedShapeType
    {
        None = 0,
        DashedLine,
        DashedDisc,
    }

    public interface IShape
    {
        public ShapeType Type { get; }
        public ShapeStyle Style { get; }
    }

    public interface IDashedShape
    {
        public DashedShapeType Type { get; }
        public ShapeStyle Style { get; }
    }

    public class ShapeStyle
    {
        public ShapesBlendMode BlendMode;
        public Color Color;
        public float Thickness;
        public ThicknessSpace ThicknessSpace;

        public ShapeStyle()
        {
            Reset();
        }
        public ShapeStyle(ShapeStyle other)
        {
            BlendMode = other.BlendMode;
            Color = other.Color;
            Thickness = other.Thickness;
            ThicknessSpace = other.ThicknessSpace;
        }

        public void Reset()
        {
            BlendMode = ShapesBlendMode.Transparent;
            Color = Color.white;
            Thickness = 0.1f;
            ThicknessSpace = ThicknessSpace.Meters;
        }
    }

    public class DashStyle
    {
        public float DashSize;
        public float DashSpacing;
        public DashSpace DashSpace;
        public DashType DashType;

        public DashStyle()
        {
            Reset();
        }
        public DashStyle(DashStyle other)
        {
            DashSize = other.DashSize;
            DashSpacing = other.DashSpacing;
            DashSpace = other.DashSpace;
            DashType = other.DashType;
        }

        public void Simple(float dashSize, float dashSpacing)
        {
            DashSize = dashSize;
            DashSpacing = dashSpacing;
        }

        public void Reset()
        {
            DashSize = 1;
            DashSpacing = 1;
            DashSpace = DashSpace.Meters;
            DashType = DashType.Basic;
        }
    }

    public class LineStyle
    {
        public Color StartColor;
        public Color EndColor;
        public LineEndCap LineEndCap;


        public LineStyle()
        {
            Reset();
        }
        public LineStyle(LineStyle other)
        {
            StartColor = other.StartColor;
            EndColor = other.EndColor;
            LineEndCap = other.LineEndCap;
        }

        public void SimpleColored(Color startColor, Color endColor)
        {
            StartColor = startColor;
            EndColor = endColor;
        }

        public void Reset()
        {
            StartColor = Color.clear;
            EndColor = Color.clear;
            LineEndCap = LineEndCap.Round;
        }
    }

    public struct Line : IShape
    {
        public Vector3 Start;
        public Vector3 End;
        public ShapeStyle ShapeStyle;
        public LineStyle LineStyle;

        public Line(Vector3 start, Vector3 end, ShapeStyle shapeStyle, LineStyle lineStyle)
        {
            Start = start;
            End = end;
            ShapeStyle = shapeStyle;
            LineStyle = lineStyle;
        }
        public ShapeType Type => ShapeType.Line;
        public ShapeStyle Style => ShapeStyle;
    }

    public struct DashedLine : IDashedShape
    {
        public Vector3 Start;
        public Vector3 End;
        public ShapeStyle ShapeStyle;
        public LineStyle LineStyle;
        public DashStyle DashStyle;
        public DashedLine(Vector3 start, Vector3 end, ShapeStyle shapeStyle, LineStyle lineStyle, DashStyle dashStyle)
        {
            Start = start;
            End = end;
            ShapeStyle = shapeStyle;
            LineStyle = lineStyle;
            DashStyle = dashStyle;
        }
        public DashedShapeType Type => DashedShapeType.DashedLine;
        public ShapeStyle Style => ShapeStyle;

    }

    public class DiscStyle
    {
        public DiscColors Colors { get; private set; }
        public bool UsingColors;
        public DiscType DiscType;

        public void SetDiscColors(DiscColors colors)
        {
            Colors = colors;
            UsingColors = true;
        }

        public DiscStyle() 
        {
            Reset();
        }
        public DiscStyle(DiscStyle other)
        {
            Colors = other.Colors;
            DiscType = other.DiscType;
        }

        public void Reset()
        {
            UsingColors = false;
            Colors = Color.clear;
            DiscType = DiscType.Disc;
        }
    }

    public struct Disc : IShape
    {
        public Vector3 Position;
        public float Radius;
        public Vector3 Normal;
        public ShapeStyle ShapeStyle;
        public DiscStyle DiscStyle;
        public Disc(Vector3 pos, float radius, Vector3 normal, ShapeStyle shapeStyle, DiscStyle discStyle)
        {
            Position = pos;
            Radius = radius;
            Normal = normal;
            ShapeStyle = shapeStyle;
            DiscStyle = discStyle;
        }


        public ShapeType Type => ShapeType.Disc;
        public ShapeStyle Style => ShapeStyle;


        public void SetNormal(Vector3 normal)
        {
            Normal = normal;
        }
    }

    public class TriangleStyle
    {
        public Vector3 LocalA;
        public Vector3 LocalB;
        public Vector3 LocalC;
        public float Roundness;

        public TriangleStyle()
        {
            Reset();
        }
        public TriangleStyle(TriangleStyle other)
        {
            LocalA = other.LocalA;
            LocalB = other.LocalB;
            LocalC = other.LocalC;
            Roundness = other.Roundness;
        }

        public void Reset()
        {
            LocalA = new Vector3(0f, 1f, 0f);
            LocalB = new Vector3(-0.57735f, 0f, 0f);
            LocalC = new Vector3(0.57735f, 0f, 0f);
            Roundness = 0.2f;
        }
    }

    public struct Triangle : IShape
    {
        public Vector3 Position;
        public Vector3 u_Heading;
        public float Size;
        public ShapeStyle ShapeStyle;
        public TriangleStyle TriangleStyle;
        public Triangle(Vector3 position, Vector3 heading, float size, ShapeStyle shapeStyle, TriangleStyle triangleStyle)
        {
            Position = position;
            u_Heading = heading;
            Size = size;
            ShapeStyle = shapeStyle;
            TriangleStyle = triangleStyle;
        }

        public ShapeType Type => ShapeType.Triangle;
        public ShapeStyle Style => ShapeStyle;


        public readonly (Vector3 A, Vector3 B, Vector3 C) GetWorldSpaceVertexPositions()
        {
            Quaternion initialRotation = Quaternion.FromToRotation(Vector3.up, u_Heading);
            Vector3 worldSpaceUp = initialRotation * Vector3.forward;

            Vector3 toCamera = Camera.main.transform.position - Position;

            Vector3 lateralToCamera = Vector3.ProjectOnPlane(toCamera, u_Heading);

            float rollAngle = Vector3.SignedAngle(worldSpaceUp, lateralToCamera, u_Heading);

            Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, u_Heading);

            Quaternion finalRotation = rollRotation * initialRotation;

            return (
                A: Position + finalRotation * TriangleStyle.LocalA * Size,
                B: Position + finalRotation * TriangleStyle.LocalB * Size,
                C: Position + finalRotation * TriangleStyle.LocalC * Size
            );
        }
    }

    public class RectangleStyle
    {
        public float BorderThickness;
        public Color BorderColor;
        public float Roundness;

        public RectangleStyle() { Reset(); }

        public RectangleStyle(RectangleStyle other)
        {
            BorderColor = other.BorderColor;
            BorderThickness = other.BorderThickness;
            Roundness = other.Roundness;
        }

        public void Reset()
        {
            BorderThickness = 0;
            BorderColor = Color.clear;
            Roundness = 0;
        }
    }

    public struct Rectangle : IShape
    {
        public ShapeStyle ShapeStyle;
        public RectangleStyle RectangleStyle;

        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Size;

        public Rectangle(Vector3 position, Vector3 normal, Vector2 size, ShapeStyle shapeStyle, RectangleStyle rectangleStyle)
        {
            Position = position;
            Normal = normal;
            Size = size;
            ShapeStyle = shapeStyle;
            RectangleStyle = rectangleStyle;
        }

        public ShapeType Type => ShapeType.Rectangle;
        public ShapeStyle Style => ShapeStyle;
    }
}
