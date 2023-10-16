using Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeType
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

public abstract class Shape : IComparable<Shape>
{
    protected ShapeType _shapeType = ShapeType.None;

    public ShapeType ShapeType { get { return _shapeType; } }

    public int CompareTo(Shape other)
    {
        if (_shapeType == other._shapeType) return 0;
        return (int)_shapeType < (int)other._shapeType ? 1 : -1;
    }
}

public struct DashData
{
    public DashData(float dashSize, float dashSpacing)
    {
        DashSize = dashSize;
        DashSpacing = dashSpacing;
        DashSpace = DashSpace.Meters;
        DashType = DashType.Basic;
    }
    public float DashSize { get; set; }
    public float DashSpacing { get; set; }
    public DashSpace DashSpace { get; set; }
    public DashType DashType { get; set; }
}
public sealed class DashedLine : Line
{
    public DashedLine(Vector3 start, Vector3 end) : base(start, end)
    {
        _shapeType = ShapeType.DashedLine;
    } 
    public DashData DashData { get; set; }
}

public class Line : Shape
{
    protected bool _endColor = false;
    public Line(Vector3 start, Vector3 end)
    {
        _shapeType = ShapeType.Line;
        Start = start; 
        End = end;
    }

    public Vector3 Start { get; set; }
    public Vector3 End { get; set; }
    public LineEndCap EndCaps { get; set; } = LineEndCap.None;
    public ThicknessSpace ThicknessSpace { get; set; } = ThicknessSpace.Meters;
    public Color ColorStart { get; set; } = Color.white;
    public Color ColorEnd { 
        get {
            if (!_endColor) return ColorStart;
            return ColorEnd;
        }
        set
        {
            _endColor = true;
            ColorEnd = value;
        }
    }
    public float Thickness { get; set; } = 0.1f;
}

public sealed class Disc : Shape
{
    public Disc
}