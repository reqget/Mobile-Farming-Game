using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();
    
    public void CropHarvestedCallBack(CropType cropType)
    {
        bool cropFound = false;

        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem item = items[i];

            if (item.cropType == cropType)
            {
                item.amount++;

                cropFound = true;

                break;
            }

        }
        //DebugInventory();

        if(cropFound) 
            return;

        items.Add(new InventoryItem(cropType,1));

    }

    public void Clear()
    {
        items.Clear();
    }

    public InventoryItem[] GetInventoryItems()
    {
        return items.ToArray();
    }

    

    public void DebugInventory()
    {
        foreach (InventoryItem item in items)
            Debug.Log("found " + item.cropType + " amount of " + item.amount);

    }
}
