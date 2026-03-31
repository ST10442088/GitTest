using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour, IGameManager
{ public ManagerStatus status { get; private set; }

    // List<string> inventoryItems;
    Dictionary<string, int> inventoryItems;
     public void Startup()
     {
        Debug.Log("The InventoryManager is starting");
        status = ManagerStatus.Started;
        inventoryItems = new Dictionary<string, int>();
     }

    void DisplayItemCount()
    {
        string inventoryItemMessage = "Item: ";
        foreach(KeyValuePair<string, int> inventoryItem in  inventoryItems)
        {
            inventoryItemMessage = inventoryItemMessage + inventoryItem;
        }
        Debug.Log(inventoryItemMessage);
    }

    public void AddItemsToInventoryList(string itemName)
    {
        if (inventoryItems.ContainsKey(itemName) == true)
        {
            inventoryItems[itemName] = inventoryItems[itemName] + 1;
        }

        else
        {
            inventoryItems[itemName] = 1;
        }
        DisplayItemCount();
    }





}




