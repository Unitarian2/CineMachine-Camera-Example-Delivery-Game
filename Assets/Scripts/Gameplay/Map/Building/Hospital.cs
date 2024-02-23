using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hospital : MonoBehaviour, IBuilding
{
    public string BuildingName => "Hospital";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(Hospital);

    public GameObject EntryPoint { get; set; }

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
