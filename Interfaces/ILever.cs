using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILever
{
    public void LeverRight();
    public void LeverLeft();
    public void LeverMiddle();
    public void Grab();
    public LeverBase.LeverType GetLeverType();
}
