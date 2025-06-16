using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;

public class InteractEvent : MonoBehaviour, ICanInteract
{
    public void Interact()
    {
        EventBus.Trigger(EventNames.MyInteractEvent, gameObject, 2);
    }
}
