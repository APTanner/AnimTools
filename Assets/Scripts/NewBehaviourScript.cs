using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimTools.Animate;
using Shapes;

public class NewBehaviourScript : Animate
{
    private Drawer _drawer;
    protected override void AnimationUpdate()
    {

    }

    protected override void AnimationStart()
    {
        _drawer = gameObject.AddComponent<Drawer>();
    }
}
