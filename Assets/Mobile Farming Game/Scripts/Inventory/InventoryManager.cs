using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDýsplay))]
public class InventoryManager : MonoBehaviour
{
    private Inventory inventory;
    private InventoryDýsplay inventoryDisplay;
    private string dataPath;

    // Start is called before the first frame update
    void Start()
    {
        dataPath = Application.persistentDataPath + "/inventoryData.txt";

        LoadInventory();
        ConfigureInventoryDisplay();

        CropTiles.onCropHarvested += CropHarvestedCallBack;
        AppleTree.onAppleHarvested += CropHarvestedCallBack;
    }
    private void OnDestroy()
    {
        CropTiles.onCropHarvested -= CropHarvestedCallBack;
        AppleTree.onAppleHarvested -= CropHarvestedCallBack;

    }

    private void ConfigureInventoryDisplay()
    {
        inventoryDisplay = GetComponent<InventoryDýsplay>();
        inventoryDisplay.Configure(inventory);
    }

    
    private void CropHarvestedCallBack(CropType cropType)
    {
        inventory.CropHarvestedCallBack(cropType);

        inventoryDisplay.UpdateDisplay(inventory);

        SaveInventory();
    }
    [NaughtyAttributes.Button]
    public void clearInventory()
    {
        inventory.Clear();
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    private void LoadInventory()
    {
        string data = "";
        if (File.Exists(dataPath))
        {
            data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);

            if(inventory == null ) 
                inventory = new Inventory();
        }
        else
        {
            File.Create(dataPath);
            inventory = new Inventory();
        }

    }

    private void SaveInventory()
    {
        string data =  JsonUtility.ToJson(inventory,true);
        File.WriteAllText(dataPath , data);
    }
}
