using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class School : Building, IBuilding
{
    public string BuildingName => "School";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(School);
    public GameObject EntryPoint { get; set; }

    public BuildingData data
    {
        get { return base.GetData(); }
        set { data = value; }
    }
    void Start()
    {
        EntryPoint = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
