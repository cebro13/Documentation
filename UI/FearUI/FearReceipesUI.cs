using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using System;

public class FearReceipesUI : MonoBehaviour, ISelectUI
{
    [SerializeField] private FearReceipeListRefSO m_fearReceipeList;
    [SerializeField] private Transform m_container;
    [SerializeField] private GameObject m_fearReceipeUiTemplate;

    private CanvasGroup m_canvasGroup;
    //private bool m_isShow;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_fearReceipeUiTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        foreach (FearReceipeRefSO fearReceipeRefSO in m_fearReceipeList.fearReceipeListRefSO)
        {
            InstantiateFearReceipeUI(fearReceipeRefSO);
        }
    }

    public void Show()
    {
        m_canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        m_canvasGroup.alpha = 0;
    }

    private void InstantiateFearReceipeUI(FearReceipeRefSO fearReceipe)
    {
        GameObject fearUiGo = Instantiate(m_fearReceipeUiTemplate, m_container);
        fearUiGo.SetActive(true);
        FearReceipeUI fearReceipeUI = fearUiGo.GetComponent<FearReceipeUI>();
        fearReceipeUI.InitializeFearUI(fearReceipe);
    }
    
    public bool IsUiActivate()
    {
        return PlayerDataManager.Instance.m_powerCanFear || Player.Instance.m_hasAllPower;
    }
}
