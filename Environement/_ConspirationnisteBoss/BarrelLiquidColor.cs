using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelLiquidColor : MonoBehaviour
{
    [SerializeField] private HasWaterColor m_hasChangeColor;
    [SerializeField] private SwitchableAudio m_switchableAudio;
    [SerializeField] private PushableGrabableBox m_pushableGrabableBoxParent;

    public CustomColor.colorIndex GetWaterColor()
    {
        return m_hasChangeColor.GetColorSettings().colorIndex;
    }

    public void MoveBarrelToAnchor(Transform anchor)
    {
        m_pushableGrabableBoxParent.MoveAtTransform(anchor);
    }

    public bool IsGrab()
    {
        return m_pushableGrabableBoxParent.IsGrab();
    }
}
