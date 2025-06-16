using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestPit_IntroPuzzle : MonoBehaviour
{
    [SerializeField] private List<ForestPit_SpiritRock> m_spritRockList;
    [SerializeField] private SwitchableDoorPersistant m_door;

    private int m_rockCounterOrder;

    private void Awake()
    {
        m_rockCounterOrder = 0;
    }

    private void Start()
    {
        HashSet<int> uniqueOrders = new HashSet<int>();
        foreach(ForestPit_SpiritRock spritRock in m_spritRockList)
        {
            int orderNumber = spritRock.GetOrder();
            // Check if the order number already exists in the HashSet
            if (!uniqueOrders.Add(orderNumber))
            {
                // If Add returns false, it means the order number is duplicated
                Debug.LogError("Une des spritRocks a le même numéro d'ordre qu'un autre");
            }

            spritRock.OnTouchRock += SpritRock_OnTouchRock;
        }
    }

    private void SpritRock_OnTouchRock(object sender, ForestPit_SpiritRock.OnTouchRockEventArgs e)
    {
        ForestPit_SpiritRock spiritRock = sender as ForestPit_SpiritRock;
        if(e.order == m_rockCounterOrder)
        {
            spiritRock.IsChanting();
            m_rockCounterOrder++;
        }
        else
        {
            spiritRock.IsDeny();
            ResetAllSpiritRocks();
            m_rockCounterOrder = 0;
        }

        if(m_rockCounterOrder == m_spritRockList.Count)
        {
            PuzzleDone();
        }
    }

    private void ResetAllSpiritRocks()
    {
        foreach(ForestPit_SpiritRock spritRock in m_spritRockList)
        {
            spritRock.ResetRock();
        }
    }

    private void PuzzleDone()
    {
        m_door.Switch();
        foreach(ForestPit_SpiritRock spritRock in m_spritRockList)
        {
            spritRock.PuzzleDone();
        }
    }
}
