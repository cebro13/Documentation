using UnityEngine;

public class GeneratedChain : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_fixationRb;
    [SerializeField] private int m_numberLink;
    [SerializeField] private GameObject[] m_prefabLinkSegment;
    [SerializeField] private HingeJoint2D m_topLink;
    [SerializeField] private float m_speed;

    private bool m_isAddingLink;
    private bool m_isRemovingLink;
    private Vector2 m_hookInitialPosition;
    private float m_distance;
    private float m_sizeOfAnchorDistanceInLink;
    

    private void Awake()
    {
        GenerateChain();
        m_isAddingLink = false;
        m_isRemovingLink = false;
        m_hookInitialPosition = transform.position;
        m_sizeOfAnchorDistanceInLink = m_prefabLinkSegment[0].GetComponentInChildren<SpriteRenderer>().bounds.size.y -0.1f;
    }

    private void GenerateChain()
    {
        Rigidbody2D previousRb = m_fixationRb;
        for(int linkIndex = 0; linkIndex < m_numberLink; linkIndex++)
        {
            int prefabIndex = UnityEngine.Random.Range(0, m_prefabLinkSegment.Length);
            GameObject newLink = Instantiate(m_prefabLinkSegment[prefabIndex]);
            newLink.transform.parent = transform;
            newLink.transform.position = transform.position;
            HingeJoint2D hingeJoint = newLink.GetComponent<HingeJoint2D>();
            hingeJoint.connectedBody = previousRb;
            previousRb = newLink.GetComponent<Rigidbody2D>();
            if(linkIndex == 0)
            {
                m_topLink = hingeJoint;
            }
        }
    }

    private void Update()
    {
        if(m_isAddingLink)
        {
            if(m_isRemovingLink)
            {
                m_isRemovingLink = false;
            }
            transform.position = new Vector2(transform.position.x, transform.position.y - m_speed * Time.deltaTime);
            m_distance += m_speed * Time.fixedDeltaTime;
            if(m_distance > m_sizeOfAnchorDistanceInLink)
            {
                //transform.position = new Vector2(transform.position.x, m_hookInitialPosition.y);
                AddLink();
                m_distance = 0f;
            }
        }

        if(m_isRemovingLink)
        {
            if(m_isAddingLink)
            {
                m_isAddingLink = false;
            }
            transform.position = new Vector2(transform.position.x, transform.position.y + m_speed * Time.deltaTime);
            m_distance -= m_speed * Time.deltaTime;
            if(m_distance < -m_sizeOfAnchorDistanceInLink)
            {
                //transform.position = new Vector2(transform.position.x, m_hookInitialPosition.y);
                RemoveLink();
                m_distance = 0f;
            }
        }
    }

    /*private void FixedUpdate()
    {
        if(m_isAddingLink)
        {
            if(m_isRemovingLink)
            {
                m_isRemovingLink = false;
            }
            m_fixationRb.MovePosition(new Vector2(m_fixationRb.position.x, m_fixationRb.position.y - m_speed * Time.fixedDeltaTime));
            m_distance += m_speed * Time.fixedDeltaTime;
            if(m_distance > m_sizeOfAnchorDistanceInLink)
            {
                m_fixationRb.transform.position = new Vector2(m_fixationRb.transform.position.x, m_hookInitialPosition.y);
                AddLink();
                m_distance = 0f;
            }
        }

        if(m_isRemovingLink)
        {
            if(m_isAddingLink)
            {
                m_isAddingLink = false;
            }
            m_fixationRb.MovePosition(new Vector2(m_fixationRb.position.x, m_fixationRb.position.y + m_speed * Time.fixedDeltaTime));
            m_distance -= m_speed * Time.deltaTime;
            if(m_distance < -m_sizeOfAnchorDistanceInLink)
            {
                m_fixationRb.transform.position = new Vector2(m_fixationRb.transform.position.x, m_hookInitialPosition.y);
                RemoveLink();
                m_distance = 0f;
            }
        }
    }*/

    public void SetIsAddingLink(bool isAddingLink)
    {
        m_isAddingLink = isAddingLink;
    }

    public void SetIsRemovingLink(bool isRemovingLink)
    {
        m_isRemovingLink = isRemovingLink;
    }

    public void AddLink()
    {
        int prefabIndex = UnityEngine.Random.Range(0, m_prefabLinkSegment.Length);
        GameObject newLink = Instantiate(m_prefabLinkSegment[prefabIndex]);
        newLink.transform.parent = transform;
        newLink.transform.position = transform.position;
        HingeJoint2D hingeJoint = newLink.GetComponent<HingeJoint2D>();
        hingeJoint.connectedBody = m_fixationRb;
        newLink.GetComponent<LinkSegment>().SetLinkBelow(m_topLink.gameObject);
        m_topLink.connectedBody = newLink.GetComponent<Rigidbody2D>();
        m_topLink.GetComponent<LinkSegment>().ResetAnchor();
        m_topLink = hingeJoint;
        m_numberLink++;
    }

    public void RemoveLink()
    {
        HingeJoint2D newTopLink = m_topLink.GetComponent<LinkSegment>().GetLinkBelow().GetComponent<HingeJoint2D>();
        newTopLink.connectedBody = m_fixationRb;
        newTopLink.transform.position = m_fixationRb.transform.position;
        newTopLink.GetComponent<LinkSegment>().ResetAnchor();
        Destroy(m_topLink.gameObject);
        m_topLink = newTopLink;
        m_numberLink--;
    }

    public int GetNumberOfLinks()
    {
        return m_numberLink;
    } 

    public void SetSpeed(float speed)
    {
        m_speed = speed;
    }
}
