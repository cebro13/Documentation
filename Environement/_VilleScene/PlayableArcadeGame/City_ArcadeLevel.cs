using UnityEngine;

public class City_ArcadeLevel : MonoBehaviour
{
    [SerializeField] private Transform m_playerStartPosition;
    [SerializeField] private City_ArcadeGameGoal m_arcadeLevelGoal;

    public Transform GetPlayerStartPosition()
    {
        return m_playerStartPosition;
    }

    public City_ArcadeGameGoal GetArcadeLevelGoal()
    {
        return m_arcadeLevelGoal;
    }

}
