using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStation : Building, IBuilding
{
    public string BuildingName => "FireStation";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(FireStation);

    public GameObject EntryPoint { get; set; }
    public BuildingData data { 
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
