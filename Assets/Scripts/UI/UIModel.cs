using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModel : MonoBehaviour
{
    [SerializeField] private BuildingList buildingTypesSO;
    private List<IBuilding> uniqueBuildingList = new();
    public IBuilding startBuilding;
    public IBuilding endBuilding;

    public event Action<Sprite,Sprite> DestinationUpdated;

    public void SetBuildingList(List<IBuilding> list)
    {
        uniqueBuildingList = list;
    }

    public void SetDeliveryDestination(DeliveryDestination deliveryDestination)
    {
        startBuilding = deliveryDestination.BuildingStart;
        endBuilding = deliveryDestination.BuildingEnd;

        ChangeDeliveryDestination();
    }

    private void ChangeDeliveryDestination()
    {
        Sprite startBuildingImage = FindSpriteOfIBuilding(startBuilding.Type);
        Sprite endBuildingImage = FindSpriteOfIBuilding(endBuilding.Type);

        DestinationUpdated?.Invoke(startBuildingImage, endBuildingImage);

    }

    private Sprite FindSpriteOfIBuilding(Type buildingTypeToSearch)
    {
        foreach(var building in uniqueBuildingList)
        {
                //Listede Type'ý bizim aradýðýmýzla ayný olan building'i buluyoruz.
                if(buildingTypeToSearch == building.Type)
                {
                    //Doðru binayý bulduk
                    return building.data.uiImage;
                }
        }

        return null;
    }
}
