using UnityEngine;

public class ShowIfAttribute : PropertyAttribute
{
    public string ConditionalSourceField { get; private set; }

    public ShowIfAttribute(string conditionalSourceField)
    {
        this.ConditionalSourceField = conditionalSourceField;
    }
}