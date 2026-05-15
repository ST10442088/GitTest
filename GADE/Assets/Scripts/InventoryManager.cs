using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    Dictionary<string, int> powerUps;

    public string itemCurrentlyEquipped {  get; private set; }
    public void DoBeforeGameStart()
    {
        powerUps = new Dictionary<string, int>();
    }

    void DisplayItems()
    {
        string powerupDisplay = "Powerup: ";
        foreach (KeyValuePair<string, int> item in powerUps)
        {
            powerupDisplay = powerupDisplay + item + " ";
        }
        Debug.Log(powerupDisplay);
    }

    public void AddItems(string powerupName)
    {

        if(powerUps.ContainsKey(powerupName))
        {
            /*The powerUpName is the key, and the value is the number of the powerupType collected
            If one of them was already collected, increase the value/amount of it, by 1 */
            powerUps[powerupName] = powerUps[powerupName] + 1;
        }
        else //If that powerupType hasn't yet been collected, the value/its amount, will be 1
        {
            powerUps[powerupName ] = 1;
        }
        DisplayItems();
    }

    public List<string> GetPowerupsList()
    {
        List<string> powerUpsList = new List<string>(powerUps.Keys); //The keys are the names of the powerupName
        return powerUpsList;
    }

    public int GetPowerup_TypeCount(string powerupName)
    {
        if (powerUps.ContainsKey(powerupName))
        {
            return powerUps[powerupName]; //return the numerical Value of the itemName/how much of the item has been collected
        }
        return 0;
    }

    public bool EquipItem(string powerupName)
    {
        if(powerUps.ContainsKey(powerupName) && itemCurrentlyEquipped != powerupName)
        {
            itemCurrentlyEquipped = powerupName;
        }
        powerupName = null;
        return true;
    }

    public bool UseEquippedItem(string powerupName)
    {
        if(powerUps.ContainsKey(powerupName))
        {
            powerUps[powerupName] = powerUps[powerupName] - 1;
        }

        if(powerUps[powerupName] == 0)
        {
            powerUps.Remove(powerupName);
        }
        else
        {
            return false;
        }

        return true;
    }
}
