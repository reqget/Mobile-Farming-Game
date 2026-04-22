using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]


public class PlayerSowAbility : MonoBehaviour
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

        SeedParticles.onSeedCollided += SeedsCollidedCallBack;

        CropField.onFullySown += CropFieldFullySownCallBack;

        playerToolSelector.onToolSelected += ToolSelectedCallBack;


    }

    private void OnDestroy()
    {
        SeedParticles.onSeedCollided -= SeedsCollidedCallBack;

        CropField.onFullySown -= CropFieldFullySownCallBack;

        playerToolSelector.onToolSelected -= ToolSelectedCallBack;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

    private void ToolSelectedCallBack(PlayerToolSelector.Tool selectedTool)
    {
        if(!playerToolSelector.canSow())
            playerAnimator.StopSowAnimation();
    }
    private void SeedsCollidedCallBack(Vector3[] seedPositions)
    {
        if (currentCropField == null)
            return;

        currentCropField.SeedsCollidedCallback(seedPositions);

    }
    private void CropFieldFullySownCallBack(CropField cropField)
    {
        if(cropField == currentCropField)
            playerAnimator.StopSowAnimation();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty() )
        {
            
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }

    }

    private void EnteredCropField(CropField cropField)
    {
        if(playerToolSelector.canSow())
        playerAnimator.PlaySowAnimation();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopSowAnimation();
            currentCropField = null;
        }
    }
}
