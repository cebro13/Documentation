
using UnityEngine;

[CreateAssetMenu(fileName = "KnowledgeUIRefSO", menuName = "Data/UI/Knowledges")]
public class KnowledgeUIRefSO : ScriptableObject
{
    public KnowledgeUI.eKnowledgeUI knowledgeUiID;
    public Sprite KnowledgeImage;
    public Sprite SmallIcon;
    public string KnowledgeTextDescription = "Lore of this object";
}
