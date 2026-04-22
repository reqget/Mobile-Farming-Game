using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject treeCam;
    [SerializeField] private Renderer Renderer;
    [SerializeField] private Transform appleParents;
    private AppleTreeManager treeManager;

    [Header("Settings")]
    [SerializeField] private float maxShakeMagnitude;
    [SerializeField] private float shakeIncrement;
    private float shakeMagnitude;
    private float shakeSliderValue;
    private bool isShaking;

    [Header("Actions")]
    public static Action<CropType> onAppleHarvested;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(AppleTreeManager treeManager)
    {
        EnableTreeCam();

        shakeSliderValue = 0;

        this.treeManager = treeManager;
    }

    public void EnableTreeCam()
    {
        treeCam.SetActive(true);
    }

    public void DisableTreeCam()
    {
        treeCam.SetActive(false);
    }

    public void Shake()
    {
        isShaking = true;

        TwenShake(maxShakeMagnitude);

        UpdateShakeSlider();
    }

    private void UpdateShakeSlider()
    {
        shakeSliderValue += shakeIncrement;
        treeManager.UpdateShakeSlider(shakeSliderValue);

        for (int i = 0; i < appleParents.childCount; i++)
        {
            float applePercent = (float)i / appleParents.childCount;

            Apple currentApple =  appleParents.GetChild(i).GetComponent<Apple>();

            if (shakeSliderValue > applePercent  && !currentApple.IsFree())
                ReleaseApple(currentApple);

            if (shakeSliderValue >= 1)
                ExitTreeMode();
        }
    }

    private  void ReleaseApple(Apple apple)
    {
        apple.Release();

        onAppleHarvested?.Invoke(CropType.Apple);

    }

    public void StopShaking()
    {
        if(!isShaking)
            return;

        isShaking = false;  

        TwenShake(0);
    }

    public bool IsReady()
    {
        for (int i = 0; i < appleParents.childCount; i++)
        if(!appleParents.GetChild(i).GetComponent<Apple>().IsReady())
                return false;

        return true;
    }

    private void TwenShake(float targetMagnitude)
    {
        LeanTween.cancel(Renderer.gameObject);
        LeanTween.value(Renderer.gameObject, UpdateShakeMagnitude, shakeMagnitude, targetMagnitude, 1);
    }

    private void UpdateShakeMagnitude(float value)
    {
      shakeMagnitude = value;
        UpdtadeMaterials();
    }

    private void UpdtadeMaterials()
    {
        Renderer.material.SetFloat("_Magnitude", shakeMagnitude);

        foreach(Transform appleT in appleParents)
        {
            Apple apple = appleT.GetComponent<Apple>();

            if(apple.IsFree())
                continue;

            apple.Shake(shakeMagnitude); 
        }
    }

    private void ExitTreeMode()
    {
        treeManager.EndTreeMode();

        DisableTreeCam();

        TwenShake(0);
        ResetApples();
    }

    private void ResetApples()
    {
        for (int i = 0; i < appleParents.childCount; i++)
            appleParents.GetChild(i).GetComponent<Apple>().Reset();
        
        
    }
}
