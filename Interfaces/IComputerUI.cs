using System;

public interface IComputerUI
{
    public event EventHandler<OnComputerEventSendEventArgs> OnEventSend;  
    public class OnComputerEventSendEventArgs : EventArgs
    {
        public ComputerOpenner.eComputerState computerState;
    }
    public void OpenComputer(ComputerOpenner.eComputerState computerState);
}
