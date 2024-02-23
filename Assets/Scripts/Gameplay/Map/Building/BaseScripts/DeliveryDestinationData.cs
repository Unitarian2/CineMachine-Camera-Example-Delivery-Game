using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DeliveryDestination
{
    public IBuilding BuildingStart { get; }
    public IBuilding BuildingEnd { get; }

    public DeliveryDestination(IBuilding buildingStart, IBuilding buildingEnd)
    {
        BuildingStart = buildingStart ?? throw new ArgumentNullException(nameof(buildingStart));
        BuildingEnd = buildingEnd ?? throw new ArgumentNullException(nameof(buildingEnd));
    }
}
