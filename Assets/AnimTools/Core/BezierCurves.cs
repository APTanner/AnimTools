using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimTools.Curves
{
    public static class BezierCurves
    {
        public readonly struct BezierPoints
        {
            public Vector2 Point1 { get; }
            public Vector2 Point2 { get; }

            public BezierPoints(Vector2 point1, Vector2 point2)
            {
                checkPointInBounds(point1);
                checkPointInBounds(point2);
                Point1 = point1;
                Point2 = point2;
            }

            private static void checkPointInBounds(Vector2 point)
            {
                if (point.x < 0 || point.x > 1 ||
                    point.y < 0 && point.y > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(point), "Bezier curve points must be in x: [0,1], y: [0,1]");
                }
            }
        }

        public static BezierPoints EaseInOut = new BezierPoints(
            new Vector2(0.42f, 0.0f),
            new Vector2(0.58f, 1.0f)
        );
    }
}
