using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog;

public class FrozenGhost : BaseNPCBehaviour, ICanInteract
{
    public void Close()
    {
        CloseDialog();
    }

    public void Interact()
    {
        
    }
}
