using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerableUI
{
    public void TriggerShow();
    public void TriggerHide();
    public bool GetIsShow();
    public bool GetIsAnimationDone();
}
