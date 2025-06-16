using System;
public interface IHasCountdown
{
    public event EventHandler<EventArgs> OnCountdownFinished;
    public event EventHandler<OnCountdownChangedEventArgs> OnCountdownChanged;  
    public class OnCountdownChangedEventArgs : EventArgs
    {
        public float countdown;
    }
    public float GetInitialCountdown();
}
