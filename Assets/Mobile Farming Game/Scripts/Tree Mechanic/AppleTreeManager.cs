using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AppleTreeManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider shakeSlider;

    [Header("Settings")]
    private AppleTree lastTriggeredTree;

    [Header("Action")]
    public static Action<AppleTree> onTreeModeStarted;
    public static Action onTreeModeEnded;

    private void Awake()
    {
        PlayerDetection.onEnteredTreeZone += EnteredTreeZoneCallBack;
    }

    private void OnDestroy()
    {
        PlayerDetection.onEnteredTreeZone -= EnteredTreeZoneCallBack;

    }

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void EnteredTreeZoneCallBack(AppleTree tree)
    {
      lastTriggeredTree = tree;
    }

    public void TreeButtonCallBack()
    {
        Debug.Log("Im A Tree Button");
        
        if (!lastTriggeredTree.IsReady()) 
            return;

        StartTreeMode();
    }

    private void StartTreeMode()
    {
        lastTriggeredTree.Initialize(this);

        onTreeModeStarted?.Invoke(lastTriggeredTree);

        UpdateShakeSlider(0);
    }

    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }

    public void EndTreeMode()
    {
        onTreeModeEnded?.Invoke();
    }
}
