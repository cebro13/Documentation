using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPosterWithLight : MonoBehaviour, ISwitchable, ICanInteract
{
    [SerializeField] private Sprite m_posterGammaRayLight;
    [SerializeField] private string m_itIsTooDarkText;
    [SerializeField] private bool m_isLight = false;

    [Header("Debug")]
    [SerializeField] private bool m_test;

    public void Interact()
    {
        if(m_isLight)
        {
            CanvasManager.Instance.OpenGrayScreenAndCheckItemUntilInput(m_posterGammaRayLight);
            
        }
        else
        {
            CanvasManager.Instance.OpenGrayScreenAndContextUntilInput(m_itIsTooDarkText);
        }
    }

    public void Switch()
    {
        m_isLight = !m_isLight;
    }

    private void Update()
    {
        if(m_test)
        {
            m_test = false;
            Switch();
        }
    }
}
