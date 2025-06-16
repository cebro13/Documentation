using UnityEngine;
using System;

public class SubclassSelectorAttribute : PropertyAttribute
{
    public Type BaseType { get; }

    public SubclassSelectorAttribute(Type baseType)
    {
        BaseType = baseType;
    }
}