using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableWaterFallSource : MonoBehaviour, ISwitchable
{
    [SerializeField] private WaterColorSettingsRefSO m_initialWaterColorSettingsRefSO;
    [SerializeField] private float m_waterAcceleration;
    [SerializeField] private GameObject m_waterFallPrefab;
    [SerializeField] private float m_waterFallWidth;
    [SerializeField] private Transform m_spawnPoint;
    [SerializeField] private bool m_canWaterChangePlayerColor;
    [SerializeField] private bool m_testSwitch;

    private SwitchableWaterFall m_switchableWaterFall;
    //private List<WaterFallSegment> m_switchableWaterFalls;

    private void Awake()
    {
        //m_switchableWaterFalls = new List<WaterFallSegment>();
        //for(int i = 0; i < m_waterFallResolution; i++)
        //{
        //    m_switchableWaterFalls.Add(new WaterFallSegment(null, i));
        //}
    }

    private void Update()
    {
        if(m_testSwitch)
        {
            Switch();
            m_testSwitch = false;
        }
    }

    public void Switch()
    {
        /*for(int i = 0; i < m_waterFallResolution; i++)
        {

        }*/
        if(m_switchableWaterFall == null)
        {
            m_switchableWaterFall = Instantiate(m_waterFallPrefab, m_spawnPoint.position, m_waterFallPrefab.transform.rotation).GetComponent<SwitchableWaterFall>();
            m_switchableWaterFall.Init(m_initialWaterColorSettingsRefSO, m_waterAcceleration, m_waterFallWidth, m_canWaterChangePlayerColor);
        }
        else
        {
            m_switchableWaterFall.Switch();
            m_switchableWaterFall = null; //So we don't switch back the water that is already falling
        }
    }

    /*public class WaterFallSegment
    {
        public SwitchableWaterFall switchableWaterFall;
        public int position;
        public WaterFallSegment(SwitchableWaterFall switchableWaterFall, int position)
        {   
            this.switchableWaterFall = switchableWaterFall;
            this.position = position;
        }
    }*/

    //private void InstantiateWaterFall()
    //{
    //    m_switchableWaterFalls.Add(Instantiate(m_waterFallPrefab, this.transform).GetComponent<SwitchableWaterFall>());
    //    m_switchableWaterFall.Init(m_initialColor, true, m_waterAcceleration, 1 << Player.GROUND_LAYER);
   // }
}