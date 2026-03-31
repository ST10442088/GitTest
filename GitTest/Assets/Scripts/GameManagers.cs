using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
public class GameManagers : MonoBehaviour
{

    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory {  get; private set; }

    List<IGameManager> gameManagersStartSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();

        gameManagersStartSequence = new List<IGameManager>();
        gameManagersStartSequence.Add(Player);
        gameManagersStartSequence.Add (Inventory);

        StartCoroutine(StartTheManagers());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartTheManagers()
    {
        foreach(IGameManager manager in gameManagersStartSequence)
        {
            manager.Startup();
        }
        yield return null;
    }
}
