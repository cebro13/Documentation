using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankableChain : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_listOfLinkOrdered;
    [SerializeField] private float m_speed;
    [SerializeField] private int m_numberOfLinksToCrankUp;
    [SerializeField] private float m_defaultConnectedAnchor;
    private int m_currentIndex;
    //private GameObject m_currentControlledLink;
    //private bool m_isAddingLink;
    //private bool m_isRemovingLink;
    //private bool m_isStartUpdate;
    private void Awake()
    {
        //m_isAddingLink = false;
        //m_isRemovingLink = false;

        if(m_listOfLinkOrdered.Count <= m_numberOfLinksToCrankUp)
        {
            Debug.LogError("The number of link to crank up must be lower than the total number of link in list of link ordered.");
        }
        m_currentIndex = m_numberOfLinksToCrankUp;
        //m_currentControlledLink = m_listOfLinkOrdered[m_currentIndex];
    }

    private void Start()
    {
        //m_isStartUpdate = true;
    }

    public void SetIsAddingLink(bool isAddingLink)
    {
        //m_isAddingLink = isAddingLink;
    }

    public void SetIsRemovingLink(bool isRemovingLink)
    {
        //m_isRemovingLink = isRemovingLink;
    }

    public void SetSpeed(float speed)
    {
        m_speed = speed;
    }
}
