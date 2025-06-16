using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoxGhost : MonoBehaviour
{
    [SerializeField] private Bowl m_bowl;
    [SerializeField] private GameObject m_canHideInFrontGameObject;

    private void Awake()
    {
        m_canHideInFrontGameObject.SetActive(false);
    }

    private void Start()
    {
        m_bowl.OnBowlFallen += Bowl_OnBowlFallen;
    }

    private void Bowl_OnBowlFallen(object sender, EventArgs e)
    {
        m_canHideInFrontGameObject.SetActive(true);
    }


}
