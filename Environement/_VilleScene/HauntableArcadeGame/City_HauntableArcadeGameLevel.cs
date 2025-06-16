using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_HauntableArcadeGameLevel : MonoBehaviour
{
    [SerializeField] private City_ArcadeGameGoal m_arcadeLevelGoal;
    [SerializeField] private List<City_HauntableArcadeGameEnemy> m_enemyList;
    [SerializeField] private City_ArcadeGamePlayer m_player;

    private int m_currentEnemyIndex;

    public City_ArcadeGameGoal GetArcadeLevelGoal()
    {
        return m_arcadeLevelGoal;
    }

    public City_HauntableArcadeGameEnemy GetFirstEnemy()
    {
        m_currentEnemyIndex = 0;
        return m_enemyList[m_currentEnemyIndex];
    }

    public City_ArcadeGamePlayer GetPlayer()
    {
        return m_player;
    }

    public City_HauntableArcadeGameEnemy GetNextEnemy(Utils.Direction direction)
    {
        Debug.Log("m_enemyList.Count : " + m_enemyList.Count);
        Debug.Log(" m_currentEnemyIndex : " + m_currentEnemyIndex);
        if(direction == Utils.Direction.Left)
        {
            if(m_currentEnemyIndex - 1 >= 0)
            {
                m_currentEnemyIndex--;
                
            }
            else
            {
                m_currentEnemyIndex = m_enemyList.Count - 1;
            }
        }
        else if(direction == Utils.Direction.Right)
        {
            if(m_currentEnemyIndex + 1 <= m_enemyList.Count - 1)
            {
                m_currentEnemyIndex++;
                
            }
            else
            {
                m_currentEnemyIndex = 0;
            }
        }
        else
        {
            Debug.LogError("Ce cas ne devrait pas arriver.");
        }
        return m_enemyList[m_currentEnemyIndex];
    }

    public void Reset()
    {
        m_player.ResetPosition();
        m_player.ResetSpeed();
        foreach(City_HauntableArcadeGameEnemy enemy in m_enemyList)
        {
            enemy.ResetPosition();
            enemy.ResetSpeed();
        }
    }
}
