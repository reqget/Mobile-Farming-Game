using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkWalls : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject backWall;
    [SerializeField] private GameObject leftWall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configure(int configuration)
    {
        frontWall.SetActive(IsBitSet(configuration, 0));
        rightWall.SetActive(IsBitSet(configuration, 1));
        backWall.SetActive(IsBitSet(configuration, 2));
        leftWall.SetActive(IsBitSet(configuration, 3));

    }

    public bool IsBitSet(int configuration , int k)
    {
        if((configuration & (1<<k)) >0)
            return false;
        else
            return true;
    }
}
