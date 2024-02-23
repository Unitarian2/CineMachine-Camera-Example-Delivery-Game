using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryDestinationManager
{
    private List<IBuilding> uniqueBuildingList;
    Type typeToAvoid = null;

    public DeliveryDestinationManager(List<IBuilding> buildings)
    {
        // Her bir binadan yalnýzca bir tane olacak þekilde filtrele
        uniqueBuildingList = buildings.GroupBy(b => b.GetType()).Select(g => g.First()).ToList();
    }

    public void UpdateTypeToAvoid(Type typeToAvoid)
    {
        this.typeToAvoid = typeToAvoid;
    }

    public DeliveryDestination GetDeliveryDestination()
    {
        
        // Liste içerisinden iki farklý bina tipi seç
        IBuilding buildingStart;
        if (typeToAvoid != null)
        {
            do
            {
                buildingStart = GetRandomBuilding();
            } while (buildingStart.Type == typeToAvoid);
        }
        else
        {
            buildingStart = GetRandomBuilding();
        }
         
        
        IBuilding buildingEnd;
        
        do
        {
            buildingEnd = GetRandomBuilding();
        } while (buildingStart.Type == buildingEnd.Type); // Ýki tip farklý olmalý

        return new DeliveryDestination(buildingStart, buildingEnd);
    }

    private IBuilding GetRandomBuilding()
    {
        int randomIndex = UnityEngine.Random.Range(0, uniqueBuildingList.Count);
        return uniqueBuildingList[randomIndex];
    }

    public List<IBuilding> GetUniqueBuildingList()
    {
        return uniqueBuildingList;
    }

}
