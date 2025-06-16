using System;
using FMODUnity;
using UnityEngine;

public class UiAudio : MonoBehaviour
{
    [SerializeField] private GameObject m_uiObject;
    [SerializeField] private EventReference m_audioAcceptUiRef;
    [SerializeField] private EventReference m_audioCancelUiRef;
    [SerializeField] private EventReference m_audioButtonSelectUiRef;

    private IUi m_ui;

    private void Awake()
    {
        m_ui = m_uiObject.GetComponent<IUi>();
        if (m_ui == null)
        {
            Debug.LogError("L'object uiObject n'hérite pas de l'interface iUi");
        }
    }

    private void Start()
    {
        m_ui.OnAcceptUi += Ui_OnAcceptUi;
        m_ui.OnCancelUi += Ui_OnCancelUi;
        m_ui.OnButtonSelectUi += Ui_OnButtonSelectUi;
    }

    private void Ui_OnAcceptUi(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioAcceptUiRef, transform.position);
    }

    private void Ui_OnCancelUi(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioCancelUiRef, transform.position);
    }
    
    private void Ui_OnButtonSelectUi(object sender, EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_audioButtonSelectUiRef, transform.position);
    }
}
