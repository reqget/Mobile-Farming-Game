using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

public class CropField : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform tilesParent;
    private List<CropTiles> cropTiles = new List<CropTiles>();

    [Header("Settings")]
    [SerializeField] private CropData cropData;
    private TileFieldState state;
    private int tilesSown;
    private int tilesWatered;
    private int tileHarvested;

    [Header("Actions")]
    public static Action<CropField> onFullySown;
    public static Action<CropField> onFullyWatered;
    public static Action<CropField> onFullyHarvested;

    // Start is called before the first frame update
    void Start()
    {
        StoreTiles();
        state = TileFieldState.Empty;
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void StoreTiles()
    {
        for (int i = 0; i< tilesParent.childCount ;i++)
            cropTiles.Add(tilesParent.GetChild(i).GetComponent<CropTiles>());
    }

    public void SeedsCollidedCallback(Vector3[] seedPositions)
    {
        for (int i = 0; i < seedPositions.Length; i++)
        {
            CropTiles closestCropTile = GetClosestCropTile(seedPositions[i]);

            if(closestCropTile == null) 
                continue;   

            if(!closestCropTile.IsEmpty())
                continue;

            Sow(closestCropTile);
        }
    }

    private void Sow(CropTiles cropTile)
    {
        cropTile.Sow(cropData);
        tilesSown++;

        if (tilesSown == cropTiles.Count)
            FieldFullySown();


    }

    public void WaterCollidedCallback(Vector3[] waterPositions)
    {
        for (int i = 0; i < waterPositions.Length; i++)
        {
            CropTiles closestCropTile = GetClosestCropTile(waterPositions[i]);

            if (closestCropTile == null)
                continue;

            if (!closestCropTile.IsSown())
                continue;

            Water(closestCropTile);

           
        }
    }

    private void Water(CropTiles cropTile)
    {
        cropTile.Water();
        tilesWatered++;

        if(tilesWatered == cropTiles.Count)
            FieldFullyWatered();
    }

   

    private void FieldFullyWatered()
    {
        Debug.Log("Watered");

        state = TileFieldState.Watered;


        onFullyWatered?.Invoke(this);
    }

    private void FieldFullySown()
    {
       

        state = TileFieldState.Sown;



        onFullySown?.Invoke(this);
    }

    public void Harvest(Transform harvestSphere)
    {
        float sphereRadius = harvestSphere.localScale.x;

        for (int i = 0; i < cropTiles.Count; i++)
        {
            if (cropTiles[i].IsEmpty())
                continue;

            float distanceCropTileSphere = Vector3.Distance(harvestSphere.transform.position, cropTiles[i].transform.position);

            if (distanceCropTileSphere <= sphereRadius)
                HarvestTile(cropTiles[i]);
        }
    }

    private void HarvestTile(CropTiles cropTile)
    {
        cropTile.Harvest();

        tileHarvested++;

        if(tileHarvested == cropTiles.Count)
            FieldFullyHarvested();
    }

    private void FieldFullyHarvested()
    {
        tilesSown = 0;
        tileHarvested = 0;
        tilesWatered = 0;

        state = TileFieldState.Empty;

        onFullyHarvested?.Invoke(this);
    }

    [NaughtyAttributes.Button]
    private void InstantlySowTiles()
    {
        for (int i = 0; i < cropTiles.Count; i++)
        {
            Sow(cropTiles[i]);
        }
    }

    [NaughtyAttributes.Button]
    private void InstantlyWaterTiles()
    {
        for (int i = 0; i < cropTiles.Count; i++)
        {
            Water(cropTiles[i]);
        }
    }
    private CropTiles GetClosestCropTile(Vector3 seedPosition)
    {
        float minDistance = 5000;
        int closestCropTileIndex = -1;


        for(int i = 0;i < tilesParent.childCount ; i++)
        {

            CropTiles cropTile = cropTiles[i];
            float distanceTileSeed = Vector3.Distance(cropTile.transform.position, seedPosition);

            if (distanceTileSeed < minDistance)
            {
                minDistance = distanceTileSeed;
                closestCropTileIndex = i;
            }

        }

        if (closestCropTileIndex == -1)
            return null;

        return cropTiles[closestCropTileIndex];

    }

    public bool IsEmpty()
    {

        return state == TileFieldState.Empty;
    }

    public bool IsSown()
    {
        return state == TileFieldState.Sown;
    }

    public bool IsWatered()
    {
        return state == TileFieldState.Watered;
    }
}
