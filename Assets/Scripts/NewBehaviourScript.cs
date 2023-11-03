using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimTools;
using AnimTools.Curves;
using AnimTools.ShapeData;

public class NewBehaviourScript : Animate
{
    protected override void AnimationUpdate()
    {
        Visualizer v = Visualizer.Instance;
        //WaitForInput();
        float t = CalculateAnimationProgress(5, EaseFunctions.Bezier, BezierCurves.EaseInOut);
        v.LineStyle = LineStyle.SimpleColored(0.1f, Color.blue, Color.blue);
        v.DrawArrow(new Vector3(), new Vector3(5, 5, 0), t);
        v.LineStyle = LineStyle.Default;
        v.DrawLine(new Vector3(), new Vector3(-1, 0, 0), t);
        v.DrawPoint(new Vector3(0,0,10), 0.5f * t, Color.green);
    }

    protected override void AnimationStart()
    {

    }
}
