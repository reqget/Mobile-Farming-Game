using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDısplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform UICropContainerParent;
    [SerializeField] private UICropContainer UICropContainerPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer cropContainerInstance = Instantiate(UICropContainerPrefab, UICropContainerParent);

            Sprite cropIcon = DataManager.instance.GetSpriteFromCropType(items[i].cropType);
            cropContainerInstance.Configure(cropIcon, items[i].amount);
        }
    }
    /*
    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        while (UICropContainerParent.childCount>0)
        {
            Transform container = UICropContainerParent.GetChild(0);
            container.SetParent(null);
            Destroy(container.gameObject);
        }
        Configure(inventory);

        /*for (int i = 0; i < items.Length; i++)
        {
            UICropContainer cropContainerInstance = Instantiate(UICropContainerPrefab, UICropContainerParent);

            Sprite cropIcon = DataManager.instance.GetSpriteFromCropType(items[i].cropType);
            cropContainerInstance.Configure(cropIcon, items[i].amount);
        }

    }*/

    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer containerInstance;

            if (i< UICropContainerParent.childCount)
            {
                 containerInstance = UICropContainerParent.GetChild(i).GetComponent<UICropContainer>();
                containerInstance.gameObject.SetActive(true);
            }
            else
                containerInstance = Instantiate(UICropContainerPrefab, UICropContainerParent);
            
            Sprite cropIcon = DataManager.instance.GetSpriteFromCropType(items[i].cropType);
            containerInstance.Configure(cropIcon, items[i].amount);


        }
        int remainingContainers = UICropContainerParent.childCount - items.Length;

        if (remainingContainers <= 0)
            return;

        for (int i = 0; i < remainingContainers; i++)
        {
            UICropContainerParent.GetChild(items.Length + i).gameObject.SetActive(false);
        }

    }

}
