using System;

public interface  IBaseDoor
{
    public event EventHandler<EventArgs> OnDoorMoveStart;
    public event EventHandler<EventArgs> OnDoorMoveStop;
}
