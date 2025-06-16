using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GenericNotImplementedError<T>
{
    static public T TryGet(T value, string name)
    {
        if(value != null)
        {
            return value;
        }

        Debug.LogError(typeof(T) + " not implemented on " + name);
        return default;
    }
}
