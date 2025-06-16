using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FearReceipeUI : MonoBehaviour
{
    [SerializeField] private Image m_topImage;
    [SerializeField] private Image m_middleImage;
    [SerializeField] private Image m_bottomImage;
    [SerializeField] private TextMeshProUGUI m_fearName;


    public void InitializeFearUI(FearReceipeRefSO fearReceipeRefSO)
    {
        m_topImage.sprite = fearReceipeRefSO.fearComponentTopRefSO.fearComponentTabSprite;
        m_middleImage.sprite = fearReceipeRefSO.fearComponentMiddleRefSO.fearComponentTabSprite;
        m_bottomImage.sprite = fearReceipeRefSO.fearComponentBottomRefSO.fearComponentTabSprite;
        m_fearName.text = fearReceipeRefSO.fearName;
    }
}
