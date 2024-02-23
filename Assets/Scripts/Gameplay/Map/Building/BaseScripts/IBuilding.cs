using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    public string BuildingName { get;}
    public GameObject GameObject { get;}
    public Type Type { get; }

    public GameObject EntryPoint { get; set; }
}
