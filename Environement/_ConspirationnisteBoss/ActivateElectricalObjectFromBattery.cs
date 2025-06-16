using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateElectricalObjectFromBattery : MonoBehaviour, IHasElectricityRunning
{
    private const string IDLE = "isIdle";
    private const string ACTIVATE = "isActivate";

    [SerializeField] private List<GameObject> m_electricGameObject;

    private List<IHasElectricityRunning> m_hasElectricityRunningList;
    private Animator m_animator;
    private bool m_isElectricityRunning;

    private bool m_isIdle;
    private bool m_isActivate;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_hasElectricityRunningList = new List<IHasElectricityRunning>();
        foreach(GameObject hasElectricityRunningGameObject in m_electricGameObject)
        {
            IHasElectricityRunning hasElectricityRunning = hasElectricityRunningGameObject.GetComponent<IHasElectricityRunning>();
            if(hasElectricityRunning != null)
            {   
                m_hasElectricityRunningList.Add(hasElectricityRunning);
            }
            else
            {
                Debug.LogError("Object " + hasElectricityRunningGameObject.name + " has no IHasElectricityRunning interface");
            }
        }
    }

    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning)
    {
        foreach(IHasElectricityRunning hasElectricityRunning in m_hasElectricityRunningList)
        {
            hasElectricityRunning.SetElectricityRunning(context, isElectricityRunning);
        }
        m_isElectricityRunning = isElectricityRunning;
        m_isIdle = !m_isElectricityRunning;
        m_isActivate = m_isElectricityRunning;
        SetAnimator();
        
        return;
    }

    private void SetAnimator()
    {
        m_animator.SetBool(IDLE, m_isIdle);
        m_animator.SetBool(ACTIVATE, m_isActivate);
    }

    public bool IsElectricityRunning()
    {
        return m_isElectricityRunning;
    }
}
