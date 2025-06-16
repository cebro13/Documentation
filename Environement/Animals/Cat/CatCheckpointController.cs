using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCheckpointController : MonoBehaviour
{
    public enum eCatState
    {
        Sit,
        JumpUp,
        JumpDown,
        Walk,
        Done
    }

    [SerializeField] private eCatState m_newCatState;
    [SerializeField] private Utils.Direction m_direction;

    public eCatState GetCatState()
    {
        return m_newCatState;
    }
    
    public Utils.Direction GetDirection()
    {
        return m_direction;
    }
}
