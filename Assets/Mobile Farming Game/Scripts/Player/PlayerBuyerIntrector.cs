using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuyerIntrector : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private InventoryManager inventoryManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buyer"))
            SellCrops();
    }

    private void SellCrops()
    {
      Inventory inventory = inventoryManager.GetInventory();
      InventoryItem[] items = inventory.GetInventoryItems();

      int coinEarnned = 0;  

    for (int i = 0; i < items.Length; i++)
        {
            int itemPrice= DataManager.instance.GetCropPriceFromCropType(items[i].cropType);
            coinEarnned += itemPrice * items[i].amount;
        }

        TransactionEffectManager.instance.PlayCoinParticles(coinEarnned);

        //CashManager.instance.AddCoins(coinEarnned);

        inventoryManager.clearInventory();
    }
}
