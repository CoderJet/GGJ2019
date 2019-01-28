using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    List<GameObject> inventory;

    void Start()
    {
        inventory = new List<GameObject>();
    }

    void Update()
    {

    }

    public void AddItemToInventory(GameObject item)
    {
        inventory.Add(item);
    }

    public void RemoveItemFromInventory(GameObject item)
    {
        inventory.Remove(item);
    }
}
