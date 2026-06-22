using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Display : MonoBehaviour
{
   [SerializeField] PlayerMovement player;
   BroomBehavior broomBehavior;



[SerializeField] int guiXposition = 50;
   [SerializeField] int guiYposition = -10;

    int guiWidth = 150;
    int guiHeight = 30;
    int buffer = 10;

    private void Start()
    {
        // broomBehavior = GetComponent<BroomBehavior>().broom;

    }

    private void OnGUI()
    {
        List<string> inventoryItemList = GameManager.InventoryManager.GetPowerupsList();
        if(inventoryItemList.Count == 0 )
        {
            GUI.Box(new Rect(guiXposition, guiYposition, guiWidth, guiHeight), "No Items Collected");
        }
        int currentXposition = guiXposition;
        foreach ( string item in inventoryItemList )
        {
            int amountOfType = GameManager.InventoryManager.GetPowerup_TypeCount(item);
            Texture2D inventoryImage = Resources.Load<Texture2D>("InventoryImages/" + item);
            GUI.Box(new Rect(currentXposition, guiYposition, guiWidth, guiHeight), new GUIContent( "("+amountOfType+")", inventoryImage));
            currentXposition = currentXposition + guiWidth + buffer;
        }

         string equippedItem = GameManager.InventoryManager.itemCurrentlyEquipped;
        if( equippedItem != null )
        {
            currentXposition = Screen.width - (guiWidth + buffer);
            Texture2D equippedImage = Resources.Load<Texture2D>("InventoryImages/" + equippedItem) as Texture2D;
            GUI.Box(new Rect(currentXposition, guiYposition, guiWidth, guiHeight), new GUIContent("Equipped", equippedImage));
        }


            currentXposition = 10;
            foreach (string itemType in inventoryItemList)
            {
                if (GUI.Button(new Rect(currentXposition, guiYposition, guiWidth, guiHeight), new GUIContent("Equip " + itemType)))
                {
                    GameManager.InventoryManager.EquipItem(itemType);
                }
                currentXposition = currentXposition + guiWidth + buffer;
            }
      
        if (equippedItem == "Phasability Device")
        {
            if (GUI.Button(new Rect(currentXposition, guiYposition, guiWidth, guiHeight), new GUIContent("Use Phasability Device")))
            {
                GameManager.InventoryManager.UseEquippedItem("Phasability Device");
                if(player != null)
                {
                 StartCoroutine(player.PhaseThrough());
                }

            }

        }

    }

    public void AssignBroomInstance(BroomBehavior broom)
    {
        broomBehavior = broom;
    }
}
