using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog;

public class DeliveryGuy : BaseNPCBehaviour, ICanInteract
{
    public void Close()
    {
        CloseDialog();
    }

    public void Interact()
    {
        OpenDialog(transform, 0, dialogs[0]);
    }
}
