using UnityEngine;

public class LeverBase : MonoBehaviour
{
    public enum LeverType
    {
        ClassicLever,
        LeverGrab,
        ThreeStateLever,
        ElectricalLever
    }
    [Header("SwitchableLeverGameObject may be null if Electrical Lever")]
    [SerializeField] protected GameObject m_switchableLeverGameObject;
    [SerializeField] private GameObject m_leverVisual;
    [SerializeField] private float m_maxAngleLever;
    [SerializeField] private float m_angleOffset = 0f;
    [SerializeField] private float m_leverSpeed;

    protected ISwitchableLever m_switchableLever;
    private float m_angleLever;
    protected bool m_leverRight;
    protected bool m_leverLeft;
    protected bool m_leverMiddle;
    protected bool m_originalPosition;
    protected bool m_leverFullySet;

    protected virtual void Awake()
    {
        m_leverRight = false;
        m_leverLeft = false;
        m_leverMiddle = true;
        m_originalPosition = true;
        m_angleLever = m_angleOffset;
        m_leverVisual.transform.rotation = Quaternion.Euler(0, 0, 0 + m_angleOffset);
        if(m_switchableLeverGameObject)
        {
            m_switchableLever = m_switchableLeverGameObject.GetComponent<ISwitchableLever>();
            if(m_switchableLever == null)
            {
                Debug.LogError("GameObjet" + m_switchableLeverGameObject + " does not have a component that implements ISwitchableLever");
            }
        }
    }

    protected virtual void Update()
    {
        if(m_angleLever > 180f)
        {
            m_angleLever -= 360f;
        }
        if(m_leverMiddle)
        {
            if(m_angleLever > 0.1f + m_angleOffset || m_angleLever < -0.1f + m_angleOffset)
            {
                m_leverVisual.transform.rotation = Quaternion.RotateTowards(m_leverVisual.transform.rotation, Quaternion.Euler(0, 0, 0 + m_angleOffset), m_leverSpeed*Time.deltaTime);
                m_originalPosition = false;
                m_leverFullySet = false;
            }
            else
            {
                m_leverVisual.transform.rotation = Quaternion.Euler(0, 0, 0 + m_angleOffset);
                m_originalPosition = true;
                m_leverFullySet = true;
            }
        }
        else if(m_leverLeft)
        {
            if(m_angleLever < m_maxAngleLever + m_angleOffset)
            {
                m_leverVisual.transform.rotation = Quaternion.RotateTowards(m_leverVisual.transform.rotation, Quaternion.Euler(0, 0, m_maxAngleLever + m_angleOffset), m_leverSpeed*Time.deltaTime);
                m_originalPosition = false;
                m_leverFullySet = false;
            }
            else
            {
                m_leverVisual.transform.rotation = Quaternion.Euler(0, 0, m_maxAngleLever + m_angleOffset);
                m_leverFullySet = true;
            }
        }
        else if(m_leverRight)
        {
            if(m_angleLever > -m_maxAngleLever + m_angleOffset)
            {
                m_leverVisual.transform.rotation = Quaternion.RotateTowards(m_leverVisual.transform.rotation, Quaternion.Euler(0, 0, -m_maxAngleLever + m_angleOffset), m_leverSpeed*Time.deltaTime);
                m_originalPosition = false;
                m_leverFullySet = false;
            }
            else
            {
                m_leverVisual.transform.rotation = Quaternion.Euler(0, 0, -m_maxAngleLever + m_angleOffset);
                m_leverFullySet = true;
            }
        }
        //Debug.Log(m_leverVisual.transform.rotation.eulerAngles.z);
        m_angleLever = m_leverVisual.transform.rotation.eulerAngles.z;
    }
}
