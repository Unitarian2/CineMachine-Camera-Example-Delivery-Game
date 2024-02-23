using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class School : MonoBehaviour, IBuilding
{
    public string BuildingName => "School";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(School);
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
