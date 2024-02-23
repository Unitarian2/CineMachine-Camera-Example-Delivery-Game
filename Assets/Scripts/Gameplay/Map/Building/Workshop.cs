using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : MonoBehaviour, IBuilding
{
    public string BuildingName => "Workshop";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(Workshop);
    public GameObject EntryPoint { get; set; }
    void Start()
    {
        EntryPoint = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
