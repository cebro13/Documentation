using System;

public interface IUi
{
    public event EventHandler<EventArgs> OnAcceptUi;
    public event EventHandler<EventArgs> OnCancelUi;
    public event EventHandler<EventArgs> OnButtonSelectUi;
}
