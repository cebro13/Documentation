using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextWriterLinesSenderRefSO", menuName = "TextWriter/TextWriterLinesSender")]
public class TextWritterLinesSenderRefSO : ScriptableObject
{
    public List<TextWritterLinesRefSO> textWritterLinesRefs;
}
