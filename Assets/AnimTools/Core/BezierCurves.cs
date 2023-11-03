using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimTools.Curves
{
    public static class BezierCurves
    {
        static readonly float CalculationTolerance = 0.0001f;

        public readonly struct BezierPoints
        {
            public readonly Vector2 Point1;
            public readonly Vector2 Point2;

            public BezierPoints(Vector2 point1, Vector2 point2)
            {
                checkPointInBounds(point1);
                checkPointInBounds(point2);
                Point1 = point1;
                Point2 = point2;
            }

            private static void checkPointInBounds(Vector2 point)
            {
                if (point.x < 0 || point.x > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(point), "Bezier curve points must be in x: [0,1], y: R");
                }
            }
        }
        public static float SolveForT(in BezierPoints points, float x)
        {
            float a = points.Point1.x;
            float b = points.Point2.x;
            // solving using newton's method
            const int maxIterations = 10;
            float tn = x;
            float xFromT = Solve(a, b, x, tn);
            for (int i = 0; i < maxIterations && Mathf.Abs(xFromT) > CalculationTolerance; i++)
            {
                float slope = (-9 * b + 9 * a + 3) * tn * tn + (6 * b - 12 * a) * tn + 3 * a;
                tn = tn - xFromT / slope;
                xFromT = Solve(a, b, x, tn);
            }
            return tn;
        }

        private static float Solve(float a, float b, float x, float tn)
        {
            return tn*tn*tn + 3 * b * (1 - tn) * tn * tn + 3 * a * (1 - tn) * (1 - tn) * tn - x;
        }

        public static float SolveForY(in BezierPoints points, float t) 
        {
            float a = points.Point1.y;
            float b = points.Point2.y;
            return t * a * 3 * (1 - t) * (1 - t) + b * 3 * (1 - t) * (t * t) + (t * t * t);
        }

        public static BezierPoints EaseInOut = new BezierPoints(
            new Vector2(0.42f, 0.0f),
            new Vector2(0.58f, 1.0f)
        );

    }
}
