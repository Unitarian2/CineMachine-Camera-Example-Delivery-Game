using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIModel Model;
    public UIView View;

    public event Action NewDeliveryRequested;

    private void OnEnable()
    {
        View.OnNewDeliveryTriggered += View_OnNewDeliveryTriggered;
        Model.DestinationUpdated += Model_DestinationUpdated;
    }

    private void OnDisable()
    {
        View.OnNewDeliveryTriggered -= View_OnNewDeliveryTriggered;
        Model.DestinationUpdated -= Model_DestinationUpdated;
    }

    public void InitUIManager(List<IBuilding> uniqueBuildingList)
    {
        Model.SetBuildingList(uniqueBuildingList);
    }

    //Yeni bir Delivery Destination geldi.
    public void NewDeliveryDestinationArrived(DeliveryDestination deliveryDestination)
    {
        Model.SetDeliveryDestination(deliveryDestination);
    }

    //Destination Model'de Set edildi.
    private void Model_DestinationUpdated(Sprite startSprite, Sprite endSprite)
    {
        View.SetDeliveryBuildingImages(startSprite, endSprite);
    }

    //Yeni bir Delivery istendi.
    private void View_OnNewDeliveryTriggered()
    {
        NewDeliveryRequested?.Invoke();
    }

    public void DeliveryCompleted()
    {
        View.RestartUIButtons();
    }
}
