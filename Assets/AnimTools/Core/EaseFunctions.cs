using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimTools.Curves
{
    public static class EaseFunctions
    {
        public static float Linear(float p)
        {
            return p;
        }

        public static float CubicOut(float p)
        {
            return 1 + Mathf.Pow(p - 1, 3);
        }

        public static float CubicIn(float p)
        {
            return Mathf.Pow(p, 3);
        }

        public static float CubicInOut(float p)
        {
            float a = Mathf.Round(p);
            return 4 * p * p * p * (1 - a) +
                (1 - 4 * Mathf.Pow(1 - p, 3)) * a;
        }

        public static float Bezier(BezierCurves.BezierPoints points, float p)
        {
            return 0;
            // TODO - function is incorrect; will need to get the y-value of the curve at a certain x-value
            // not a trivial thing to do
            //return 0 +
            //    3 * Mathf.Pow(1 - p, 2) * p * controlPoints.x +
            //    3 * (1 - p) * p * p * controlPoints.y +
            //    p * p * p;
        }
    }
}
