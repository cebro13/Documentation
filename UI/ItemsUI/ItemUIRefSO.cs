
using UnityEngine;

[CreateAssetMenu(fileName = "ItemUIRefSO", menuName = "Data/UI/Items")]
public class ItemUIRefSO : ScriptableObject
{
    public ItemUI.eItemUI itemUIID;
    public Sprite ItemImage;
    public Sprite SmallIcon;
    public string ItemTextDescription = "Lore of this object";
}
