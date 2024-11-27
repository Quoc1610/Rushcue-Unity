using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObs : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject fallTreePrefab;

    public void fallTree()
    {
        treePrefab.SetActive(false);
        fallTreePrefab.SetActive(true);
    }
}