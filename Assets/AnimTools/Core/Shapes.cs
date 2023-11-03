using JetBrains.Annotations;
using Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace AnimTools.ShapeData
{
    public enum ShapeType : int
    {
        None = 0,
        DashedLine,
        DashedDisc,
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
    public static class ShapeInfo
    {
        public const ShapeType LastDashedShape = ShapeType.DashedDisc;
    }

    public interface IShape : IComparable<IShape>
    {
        public ShapeType Type { get; }
    }

    public struct DashStyle
    {
        public float DashSize;
        public float DashSpacing;
        public DashSpace DashSpace;
        public DashType DashType;

        public DashStyle(float dashSize, float dashSpacing, DashSpace dashSpace, DashType dashType)
        {
            DashSize = dashSize;
            DashSpacing = dashSpacing;
            DashSpace = dashSpace;
            DashType = dashType;
        }

        public static DashStyle Default => new DashStyle(1, 1, DashSpace.Meters, DashType.Basic);
        public static DashStyle Simple(float dashSize, float dashSpacing) => new DashStyle(dashSize, dashSpacing, DashSpace.Meters, DashType.Basic);
        
    }

    public struct LineStyle
    {
        public float Width;
        public Color StartColor;
        public Color EndColor;
        public LineEndCap LineEndCap;
        public ThicknessSpace ThicknessSpace;

        public LineStyle(float width, Color startColor, Color endColor, LineEndCap lineEndCap, ThicknessSpace thicknessSpace)
        {
            Width = width;
            StartColor = startColor;
            EndColor = endColor;
            LineEndCap = lineEndCap;
            ThicknessSpace = thicknessSpace;
        }

        public static LineStyle Default => SimpleColored(0.1f, Color.white, Color.white);
        public static LineStyle SimpleColored(float width, Color startColor, Color endColor) => new LineStyle(width, startColor, endColor, LineEndCap.Round, ThicknessSpace.Meters);
        public static LineStyle Simple(float width) => SimpleColored(width, Color.white, Color.white);
        public static implicit operator LineStyle(Color color) => SimpleColored(0.1f, color, color);
    }

    public struct Line : IShape
    {
        public Vector3 Start;
        public Vector3 End;
        public LineStyle LineStyle;

        public Line(Vector3 start, Vector3 end, LineStyle lineStyle)
        {
            Start = start;
            End = end;
            LineStyle = lineStyle;
        }
        public static Line Simple(Vector3 start, Vector3 end) => new Line(start, end, LineStyle.Default);
        public ShapeType Type => ShapeType.Line;
        public int CompareTo(IShape other)
        {
            if (Type == other.Type) return 0;
            return (int)Type < (int)other.Type ? 1 : -1;
        }
    }

    public struct DashedLine : IShape
    {
        public Vector3 Start;
        public Vector3 End;
        public LineStyle LineStyle;
        public DashStyle DashStyle;
        public DashedLine(Vector3 start, Vector3 end, LineStyle lineStyle, DashStyle dashStyle)
        {
            Start = start;
            End = end;
            LineStyle = lineStyle;
            DashStyle = dashStyle;
        }
        public static DashedLine Simple(Vector3 start, Vector3 end) => new DashedLine(start, end, LineStyle.Default, DashStyle.Default);
        public ShapeType Type => ShapeType.DashedLine;
        public int CompareTo(IShape other)
        {
            if (Type == other.Type) return 0;
            return (int)Type < (int)other.Type ? 1 : -1;
        }
    }

    public struct DiscStyle
    {
        public DiscColors Colors;
        public DiscStyle(DiscColors colors) {  Colors = colors; }
        public static DiscStyle Default => SingleColor(Color.white);
        public static DiscStyle SingleColor(Color color) => new DiscStyle(color);
    }

    public struct Disc : IShape
    {
        public Vector3 Position;
        public float Radius;
        public Vector3 Normal;
        public DiscStyle DiscStyle;
        public Disc(Vector3 pos, float radius, Vector3 normal, DiscStyle discStyle)
        {
            Position = pos;
            Radius = radius;
            Normal = normal;
            DiscStyle = discStyle;
        }

        public ShapeType Type => ShapeType.Disc;

        public static Disc Simple(Vector3 position, float radius) => SimpleFacing(position, radius, (Camera.main.transform.position - position).normalized);
        public static Disc SimpleFacing(Vector3 position, float radius, Vector3 normal) => new Disc(position, radius, normal, DiscStyle.Default);
        public static Disc SingleColor(Vector3 position, float radius, Color color) => new Disc(position, radius, (Camera.main.transform.position - position).normalized, DiscStyle.SingleColor(color));

        public int CompareTo(IShape other)
        {
            if (Type == other.Type) return 0;
            return (int)Type < (int)other.Type ? 1 : -1;
        }
    }

    public struct TriangleStyle
    {
        public Vector3 LocalA;
        public Vector3 LocalB;
        public Vector3 LocalC;
        public Color Color;
        public float Roundness;

        public TriangleStyle(Vector3 a, Vector3 b, Vector3 c, Color color, float roundness)
        {
            LocalA = a;
            LocalB = b;
            LocalC = c;
            Color = color;
            Roundness = roundness;
        }

        public static TriangleStyle Default => SingleColor(Color.white);
        public static TriangleStyle Simple(Color color, float roundness) => new TriangleStyle(new Vector3(0f, 1f, 0f), new Vector3(-0.57735f, 0f, 0f), new Vector3(0.57735f, 0f, 0f), color, roundness);
        public static TriangleStyle SingleColor(Color color) => Simple(color, 0.2f);
    }

    public struct Triangle : IShape
    {
        public Vector3 Position;
        public Vector3 u_Heading;
        public float Size;
        public TriangleStyle TriangleStyle;
        public Triangle(Vector3 position, Vector3 heading, float size, TriangleStyle triangleStyle)
        {
            Position = position;
            u_Heading = heading;
            Size = size;
            TriangleStyle = triangleStyle;
        }

        public ShapeType Type => ShapeType.Triangle;

        public static Triangle Simple(Vector3 position, Vector3 u_heading, float size) => SimpleColored(position, u_heading, size, Color.white);
        public static Triangle SimpleColored(Vector3 position, Vector3 u_heading, float size, Color color) => new Triangle(
            position, u_heading, size, TriangleStyle.SingleColor(color)
        );

        public int CompareTo(IShape other)
        {
            if (Type == other.Type) return 0;
            return (int)Type < (int)other.Type ? 1 : -1;
        }

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
}
