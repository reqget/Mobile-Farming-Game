using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]


public class PlayerHarvestAbility : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform harvestSphere;
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;


    [Header("Settings")]
    private CropField currentCropField;
    private bool canHarvest;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

       // WaterParticles.onWaterCollided += WaterCollidedCallBack;


        CropField.onFullyHarvested += CropFieldFullyHarvestedCallBack;

        playerToolSelector.onToolSelected += ToolSelectedCallBack;


    }

    private void OnDestroy()
    {
        //WaterParticles.onWaterCollided -= WaterCollidedCallBack;

        CropField.onFullyHarvested -= CropFieldFullyHarvestedCallBack;

        playerToolSelector.onToolSelected -= ToolSelectedCallBack;


    }

    // Update is called once per frame
    void Update()
    {

    }


    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if (!playerToolSelector.canHarvest())
            playerAnimator.StopHarvestAnimation();
    }
  
    private void CropFieldFullyHarvestedCallBack(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopHarvestAnimation();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
        {

            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }

    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.canHarvest())
        {
            if (currentCropField == null)
                currentCropField = cropField;


            playerAnimator.PlayHarvestAnimation();

            if(canHarvest)
                currentCropField.Harvest(harvestSphere);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopHarvestAnimation();
            currentCropField = null;
        }
    }

    public void HarvestingStartedCallBack()
    {
        canHarvest =true;
    }
    public void HarvestingStoppedCallBack()
    {
        canHarvest =false;  
    }

}
