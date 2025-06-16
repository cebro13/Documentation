using System;
using UnityEngine;

public class FearMaterialization : MonoBehaviour
{
    public event EventHandler<EventArgs> OnStart;
    private bool m_startAudio = true;

    private void FearMaterializationAnimationDone()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (m_startAudio)
        {
            m_startAudio = false;
            OnStart?.Invoke(this, EventArgs.Empty);
        }
    }
}
