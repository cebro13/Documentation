using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimator : MonoBehaviour, ISwitchable
{
    [SerializeField] private bool m_test;

    public void Switch()
    {
        GetComponent<Animator>().enabled = !GetComponent<Animator>().enabled;
    }

    private void Update()
    {
        if(m_test)
        {
            Switch();
            m_test = false;
        }
    }
}
