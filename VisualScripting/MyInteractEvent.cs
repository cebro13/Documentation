using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[UnitTitle("On my Interact Event")]//The Custom Scripting Event node to receive the Event. Add "On" to the node title as an Event naming convention.
[UnitCategory("Events\\MyEvents")]//Set the path to find the node in the fuzzy finder as Events > My Events.
public class MyInteractEvent : GameObjectEventUnit<int>
{
  public override Type MessageListenerType => typeof(InteractMessageListener);
  protected override string hookName => EventNames.MyInteractEvent;
  
  [DoNotSerialize]// No need to serialize ports.
  public ValueOutput result { get; private set; }// The Event output data to return when the Event is triggered.

  protected override void Definition()
  {
      base.Definition();
      // Setting the value on our port.
      result = ValueOutput<int>(nameof(result));
  }
  // Setting the value on our port.
  protected override void AssignArguments(Flow flow, int data)
  {
      flow.SetValue(result, data);
  }
}