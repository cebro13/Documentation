using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    public void Push(int dir);
    public void StopPush();
    public bool IsPlayerInPushingRange();
}
