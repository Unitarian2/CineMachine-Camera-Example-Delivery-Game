using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceStation : MonoBehaviour,IBuilding
{
    public string BuildingName => "PoliceStation";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(PoliceStation);
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
