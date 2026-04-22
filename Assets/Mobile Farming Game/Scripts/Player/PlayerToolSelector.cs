using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerToolSelector : MonoBehaviour
{
    public enum Tool { None,Sow,Water,Harvest }
    private Tool activeTool;

    [Header("Elements")]
    [SerializeField] private Image[] toolImages;

    [Header("Settings")]
    [SerializeField] private Color SelectedToolColor;

    [Header("Actions")]
    public Action<Tool> onToolSelected;

    // Start is called before the first frame update
    void Start()
    {
        SelectTool(0);
    }

   

    public void SelectTool(int toolIndex)
    {
        activeTool = (Tool)toolIndex;
       
        for (int i = 0; i < toolImages.Length; i++)
        {
            toolImages[i].color = i == toolIndex ? SelectedToolColor : Color.white;
        }

        onToolSelected?.Invoke(activeTool);

    }

    public bool canSow()
    {
        return activeTool == Tool.Sow;
    }

    public bool canWater()
    {
        return activeTool == Tool.Water;
    }

    public bool canHarvest()
    {
        return activeTool == Tool.Harvest;
    }

}
