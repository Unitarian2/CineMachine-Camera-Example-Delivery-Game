using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BuildingFinder
{
    private IBuilding targetBuildingType;
    private List<IBuilding> buildingList;
    
    private List<IBuilding> possibleTargetBuildingList;
    public BuildingFinder(IBuilding targetBuildingType, List<IBuilding> buildingList)
    {
        this.targetBuildingType = targetBuildingType;
        this.buildingList = buildingList;
        
    }

    public BuildingFinder FindSameBuildingsByType()
    {
        possibleTargetBuildingList = new();
        foreach (IBuilding building in buildingList)
        {
            //TargetBuilding'in tipinde olan building'lerle iþimiz sadece
            if (building.Type == targetBuildingType.Type)
            {
                possibleTargetBuildingList.Add(building);
            }
        }

        if (possibleTargetBuildingList.Count == 0)
        {
            possibleTargetBuildingList = null;
            return this;
        }
        
        
        return this;
    }

    public IBuilding FindClosestBuilding(Vector3 sourcePos)
    {
        if (possibleTargetBuildingList == null)
        {
            return null; // Eðer null dönmüþse, diðer fonksiyonlar çaðrýlamaz
        }

        IBuilding closestBuilding = null;
        float closestPathDistance = Mathf.Infinity;
        foreach (IBuilding possibleBuilding in possibleTargetBuildingList)
        {
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(sourcePos, possibleBuilding.EntryPoint.transform.position, NavMesh.AllAreas, path);

            float pathDistance = GetPathDistance(path);

            if ((pathDistance < closestPathDistance))
            {
                closestPathDistance = pathDistance;
                closestBuilding = possibleBuilding;
            }
        }

        if (closestBuilding == null) return null;
        return closestBuilding;
    }

    private float GetPathDistance(NavMeshPath path)
    {
        float distance = 0f;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return distance;
    }
}

