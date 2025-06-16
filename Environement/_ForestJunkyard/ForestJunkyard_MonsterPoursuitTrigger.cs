using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestJunkyard_MonsterPoursuitTrigger : MonoBehaviour
{
    [SerializeField] private ForestJunkyard_MonsterPoursuit m_forestJunkyardMonsterPoursuit;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_forestJunkyardMonsterPoursuit.Poursuit();
        }
    }
}
