using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkSegment : MonoBehaviour
{

    [SerializeField] private GameObject m_linkAbove;
    [SerializeField] private GameObject m_linkBelow;

    // Start is called before the first frame update
    void Start()
    {
        ResetAnchor();
    }

    public void ResetAnchor()
    {
        m_linkAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        if(m_linkAbove.TryGetComponent<LinkSegment>(out LinkSegment linkAbove))
        {
            linkAbove.SetLinkBelow(this.gameObject);
            float sizeSpriteBelow = m_linkAbove.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, sizeSpriteBelow*-0.9f);
        }
        else
        {
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        }
    }

    public float GetSpriteSize() //TODO NB
    {
        return m_linkAbove.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
    }

    public void SetLinkBelow(GameObject gameObject)
    {
        m_linkBelow = gameObject;
    }

    public GameObject GetLinkBelow()
    {
        return m_linkBelow;
    }
}
