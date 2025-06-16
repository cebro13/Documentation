using System;


public interface ISwitchableLever
{
    public event EventHandler<EventArgs> OnActionStart;
    public event EventHandler<EventArgs> OnActionStop;

    public void LeverRight();
    public void LeverLeft();
    public void LeverMiddle();
    public void Grab();
}
