using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOpenGrayScreenAndCheckItemUntilInput : MonoBehaviour
{
    [SerializeField] private Sprite m_newItem;
    [SerializeField] private Dialog.ScriptableObjects.Lines line;

    private void OnEnable()
    {

        if (line != null)
        {
            line.OnLineClosed  += NewItem;
        }
    }

    private void OnDisable()
    {
        if (line != null)
        {
            line.OnLineClosed -= NewItem;
        }
    }

        public void NewItem()
    {
        CanvasManager.Instance.OpenGrayScreenAndCheckItemUntilInput(m_newItem);
    }
}
