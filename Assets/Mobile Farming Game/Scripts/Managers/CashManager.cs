using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;

    [Header("Settings")]
    private int coins;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
        UpdateCoinContainer();
    }

    // Start is called before the first frame update
    void Start()
    {
        Add500Coins();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetCoins()
    {
        return coins;
    }
    [NaughtyAttributes.Button]
    private void Add500Coins()
    {
        AddCoins(500);
    }

    public void UseCoins(int amount)
    {
        AddCoins(-amount);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinContainer();

        SaveData();
    }
    private void UpdateCoinContainer()
    {
        GameObject[] coinContainers = GameObject.FindGameObjectsWithTag("CoinAmount");

        foreach (GameObject coinContainer in coinContainers) 
            coinContainer.GetComponent<TextMeshProUGUI>().text = coins.ToString();
    }

    private void LoadData()
    {
      coins =  PlayerPrefs.GetInt("Coins");
    }


    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins",coins);  
    }
}
