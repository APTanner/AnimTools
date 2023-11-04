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

        float t = CalculateAnimationProgress(1, EaseFunctions.CubicOut);
        Vector3 origin = Vector3.zero;
        float extent = 10;
        float gridSpacing = .5f;
        v.LineStyle = LineStyle.Default;
        v.DrawArrow(origin, origin + new Vector3(extent + 0.5f, 0f, 0f), t);
        v.DrawArrow(origin, origin + new Vector3(0f, extent + 0.5f, 0f), t);
        for (int i = 1; i <= extent / gridSpacing; i++)
        {
            float distFromOrigin = i * gridSpacing;
            float pAlongLine = distFromOrigin / extent;
            Wait(gridSpacing / extent * 1);
            float lineP = CalculateAnimationProgress(1, EaseFunctions.CubicOut);
            v.LineStyle = LineStyle.Simple(0.02f);
            // x-y
            v.DrawLine(new Vector3(origin.x + distFromOrigin, origin.y, origin.z), 
                new Vector3(origin.x + distFromOrigin, origin.y + extent, origin.z), lineP);
            v.DrawLine(new Vector3(origin.x, origin.y + distFromOrigin, origin.z), 
                new Vector3(origin.x + extent, origin.y + distFromOrigin, origin.z), lineP);
        }
        Wait(1f);
        t = CalculateAnimationProgress(1, EaseFunctions.CubicOut);
        v.LineStyle = LineStyle.Default;
        v.DrawArrow(origin, origin + new Vector3(0f, 0f, extent + 0.5f), t);
        for (int i = 1; i <= extent / gridSpacing; i++)
        {
            float distFromOrigin = i * gridSpacing;
            float pAlongLine = distFromOrigin / extent;
            Wait(gridSpacing / extent * 1);
            float lineP = CalculateAnimationProgress(1, EaseFunctions.CubicOut);
            v.LineStyle = LineStyle.Simple(0.02f);
            // x-z
            v.DrawLine(new Vector3(origin.x + distFromOrigin, origin.y, origin.z),
                new Vector3(origin.x + distFromOrigin, origin.y, origin.z + extent), lineP);
            v.DrawLine(new Vector3(origin.x, origin.y, origin.z + distFromOrigin),
                new Vector3(origin.x + extent, origin.y, origin.z + distFromOrigin), lineP);
            // y-z
            v.DrawLine(new Vector3(origin.x, origin.y + distFromOrigin, origin.z),
                new Vector3(origin.x, origin.y + distFromOrigin, origin.z + extent), lineP);
            v.DrawLine(new Vector3(origin.x, origin.y, origin.z + distFromOrigin),
                new Vector3(origin.x, origin.y + extent, origin.z + distFromOrigin), lineP);
        }

    }

    protected override void AnimationStart()
    {

    }
}
