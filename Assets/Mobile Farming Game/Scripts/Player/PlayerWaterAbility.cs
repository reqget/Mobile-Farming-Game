using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]


public class PlayerWaterAbility : MonoBehaviour
{
    [Header("Elements")]
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;


    [Header("Settings")]
    private CropField currentCropField;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();
        WaterParticles.onWaterCollided += WaterCollidedCallBack;


        CropField.onFullyWatered += CropFieldFullyWateredCallBack;

        playerToolSelector.onToolSelected += ToolSelectedCallBack;


    }

    private void OnDestroy()
    {
        WaterParticles.onWaterCollided -= WaterCollidedCallBack;

        CropField.onFullyWatered -= CropFieldFullyWateredCallBack;

        playerToolSelector.onToolSelected -= ToolSelectedCallBack;


    }

    // Update is called once per frame
    void Update()
    {

    }


    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if (!playerToolSelector.canWater())
            playerAnimator.StopWaterAnimation();
    }
    private void WaterCollidedCallBack(Vector3[] waterPositions)
    {
        if (currentCropField == null)
            return;

        currentCropField.WaterCollidedCallback(waterPositions);

    }
    private void CropFieldFullyWateredCallBack(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopWaterAnimation();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsSown())
        {

            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }

    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.canWater())
        {
            if (currentCropField == null)
                currentCropField = cropField;


            playerAnimator.PlayWaterAnimation();


        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsSown())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopWaterAnimation();
            currentCropField = null;
        }
    }
}
