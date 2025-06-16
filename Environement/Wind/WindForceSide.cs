using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForceSide : MonoBehaviour
{
    [SerializeField] private float m_strength = 35f;
    [TextArea]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Note = "Direction du vent. 1 = droite, -1 = gauche";
    [SerializeField] private int m_direction = 1;

    private void Awake()
    {
        if(m_direction != 1 && m_direction != -1)
        {
            Debug.LogError("La direction du vent n'est pas bonne! Elle doit être de 1 pour la droite, ou de -1 pour la gauche");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetLimitMoveSpeed(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.Core.GetCoreComponent<PlayerMovement>().AddForceContinuous(new Vector2(m_strength * m_direction, 0f));
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetLimitMoveSpeed(false);
        }
    }
}
