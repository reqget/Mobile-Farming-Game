using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
[RequireComponent(typeof(ChunkWalls))]
public class Chunk : MonoBehaviour
{
    [Header("Elemetns")]
    [SerializeField] private GameObject unlockedElements;
    [SerializeField] private GameObject lockedElements;
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private MeshFilter chunkFilter;
    private ChunkWalls chunkWalls;

    [Header("Settings")]
    [SerializeField] private int initialPrice;
    private int currentPrice;
    private bool unlocked;

    [Header("Actions")]
    public static Action onUnlocked;
    public static Action onPriceChanged;
    // Start is called before the first frame update
    void Start()
    {
       
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
           chunkWalls = GetComponent<ChunkWalls>();
    }

    public void Initialize(int loadPrice)
    {
        currentPrice = loadPrice;
        priceText.text = currentPrice.ToString();

        if(currentPrice <= 0)
        Unlock(false);
    }

    public void TryUnlock()
    {
        if (CashManager.instance.GetCoins() <= 0)
            return;

        currentPrice--;
        CashManager.instance.UseCoins(1);

        onPriceChanged?.Invoke();

        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
            Unlock();
    }

    private void Unlock(bool triggerAction = true)
    {
        unlockedElements.SetActive(true);
        lockedElements.SetActive(false);

        unlocked = true;  
        
        if (triggerAction)
         onUnlocked?.Invoke();
    }

    public void UpdateWalls(int configuration)
    {
        chunkWalls.Configure(configuration);
    }

    public void DisplayLockedElements()
    {
        lockedElements.SetActive(true);
    }

    public void SetRenderer(Mesh chunkMesh)
    {
        chunkFilter.mesh = chunkMesh; 
    }

    public bool Isunlocked()
    {
        return unlocked;
    }
    public int GetInitialPrice()
    {
        return initialPrice;
    }


    public int GetCurrentPrice()
    {
        return currentPrice;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 5);

        Gizmos.color = new Color(0, 0, 0, 0);
        Gizmos.DrawCube(transform.position, Vector3.one * 5);
    }

}



