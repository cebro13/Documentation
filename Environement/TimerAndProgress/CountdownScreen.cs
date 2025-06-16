using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownScreen : MonoBehaviour
{
    [SerializeField] private GameObject m_hasCountdownGameObject;
    [SerializeField] private TextMeshPro m_countdownText;

    private IHasCountdown m_hasCountdown;

    // Start is called before the first frame update
    void Start()
    {
        m_hasCountdown = m_hasCountdownGameObject.GetComponent<IHasCountdown>();
        if(m_hasCountdown == null)
        {
            Debug.LogError("GameObjet" + m_hasCountdownGameObject + " does not have a component that implements IHasCountdown");
        }
        m_hasCountdown.OnCountdownChanged += HasCountdown_OnCountdownChanged;
        m_countdownText.text = m_hasCountdown.GetInitialCountdown().ToString();
    }

    private void HasCountdown_OnCountdownChanged(object sender, IHasCountdown.OnCountdownChangedEventArgs e)
    {
        m_countdownText.text = e.countdown.ToString();
    }

}
