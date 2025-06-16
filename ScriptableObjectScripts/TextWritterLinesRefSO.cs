using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextWriterLinesRefSO", menuName = "Data/TextWriter")]
public class TextWritterLinesRefSO : ScriptableObject
{
    public List<TextWriter.TextWriterLine> textWriterLines;
}
