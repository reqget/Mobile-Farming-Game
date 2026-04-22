using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTester : MonoBehaviour
{
    public Action myAction;


    // Start is called before the first frame update
    void Start()
    {
        //myAction = DebugNumber;
        //myAction += DebugString;

        myAction?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DebugNumber()
    {
        Debug.Log("5");
    }

    private void DebugString()
    {
        Debug.Log("Hello World");
    }


}
