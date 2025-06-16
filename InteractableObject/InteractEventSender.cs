using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractEventSender : MonoBehaviour, ICanInteract
{
    public event EventHandler<EventArgs> OnInteract;

    public void Interact()
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }
}