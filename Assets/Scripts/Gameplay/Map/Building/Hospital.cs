using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hospital : Building, IBuilding
{
    public string BuildingName => "Hospital";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(Hospital);

    public GameObject EntryPoint { get; set; }

    public BuildingData data
    {
        get { return base.GetData(); }
        set { data = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        EntryPoint = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
