using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearCharm : MonoBehaviour
{
    public GameObject charmEffects;

    public void TriggerFearUI()
    {
        FearUI.Instance.Show();
    }

    private void Start()
    {
        if (PlayerDataManager.Instance.m_powerCanFear || Player.Instance.m_hasAllPower)
        {
            charmEffects.SetActive(true);
        }
        else
        {
            PlayerDataManager.Instance.OnNewPowerFound += PlayerDataManager_OnNewPowerFound;
            charmEffects.SetActive(false);
        }
    }

    private void PlayerDataManager_OnNewPowerFound(object sender, PlayerDataManager.OnNewPowerFoundEventArg e)
    {
        if (e.powerUp == PowerUp.CanFear)
        {
            charmEffects.SetActive(true);
            PlayerDataManager.Instance.OnNewPowerFound -= PlayerDataManager_OnNewPowerFound;
        }
    }

}
