using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActiveGameObjectList : MonoBehaviour, ICanInteract, ISwitchable
{
    [SerializeField] private List<GameObject> m_gameObjects;

    private void SwitchAll()
    {
        foreach(GameObject gameObject in m_gameObjects)
        {
            if(gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }

    public void Interact()
    {
        SwitchAll();
    }

    public void Switch()
    {
        SwitchAll();
    }
}