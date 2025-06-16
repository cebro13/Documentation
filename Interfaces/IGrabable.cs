using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabable
{
    public bool GrabbedBy(HandleGrabable handleGrabable);
    public void ReleasedBy(HandleGrabable handleGrabable);
    public bool IsAtAnchor();
    public Utils.GroundStable IsGroundStable();
}
