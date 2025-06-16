using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayingInformationUI : MonoBehaviour, IDataPersistant
{
    public static GamePlayingInformationUI Instance {get; private set;}

    [SerializeField] TextMeshProUGUI m_numberOfPurpleCircleText;
    [SerializeField] TextMeshProUGUI m_numberOfDiamondText;

    private int m_numberOfPurpleCircle;
    private int m_numberOfDiamond;

    private void Awake()
    {
        Instance = this;
        CircleCollectable.OnCircleCollectableTouched += CircleCollectable_OnCircleCollectableTouched;
        DiamondCollectable.OnDiamondCollectableTouched += DiamondCollectable_OnDiamondCollectableTouched;
    }

    private void CircleCollectable_OnCircleCollectableTouched(object sender, System.EventArgs e)
    {
        m_numberOfPurpleCircle++;
        UpdateVisualPurpleCircle();
    }

    private void DiamondCollectable_OnDiamondCollectableTouched(object sender, System.EventArgs e)
    {
        m_numberOfDiamond++;
        UpdateVisualDiamond();
    }

    public void LoadData(GameData data)
    {
     //   m_numberOfPurpleCircle = data.numberOfPurpleCircle;
    //    foreach(KeyValuePair<string, bool> pair in data.diamondCollected)
        {
     //       if(pair.Value)
            {
                m_numberOfDiamond++;
            }
        }
        UpdateVisualPurpleCircle();
        UpdateVisualDiamond();
    }

    public void SaveData(GameData data)
    {
  //      data.numberOfPurpleCircle = m_numberOfPurpleCircle;
    }

    private void Start()
    {
        UpdateVisualPurpleCircle();
        UpdateVisualDiamond();
    }

    

    private void UpdateVisualPurpleCircle()
    {
        m_numberOfPurpleCircleText.text = "Purple circle: " + m_numberOfPurpleCircle;
    }

    private void UpdateVisualDiamond()
    {
        m_numberOfDiamondText.text = "Diamond found: " + m_numberOfDiamond + " / 2";
    }

    
}
