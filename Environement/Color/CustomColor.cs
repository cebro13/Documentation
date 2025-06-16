using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomColor
{
    public CustomColor(colorIndex colorIndex, Color color)
    {
        index = colorIndex;
        this.color = color;
    }

    public enum colorIndex
    {
        RED,
        BLUE,
        VIOLET,
        BLANK,
        GREEN
    }

    public colorIndex index;
    public Color color;

}
