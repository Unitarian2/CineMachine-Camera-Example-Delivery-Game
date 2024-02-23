using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private List<IBuilding> buildingList = new();
    [SerializeField] private Transform buildingParentObject;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIManager uiManager;

    DeliveryDestinationManager deliveryDestinationManager;


    private void OnDestroy()
    {
        uiManager.NewDeliveryRequested -= UiManager_NewDeliveryRequested;
    }

    void Start()
    {
        foreach (Transform childTransform in buildingParentObject)
        {
            if (childTransform.gameObject.TryGetComponent<IBuilding>(out IBuilding building))
            {
                buildingList.Add(building);
            }
        }

        
        InitDeliverySystem();
        //StartCoroutine(DoSomething());
        
    }
    
    private void InitDeliverySystem()
    {
        deliveryDestinationManager = new DeliveryDestinationManager(buildingList);
        uiManager.InitUIManager(deliveryDestinationManager.GetUniqueBuildingList());
        playerController.InitPlayerController(this);
        uiManager.NewDeliveryRequested += UiManager_NewDeliveryRequested;
    }

    private void UiManager_NewDeliveryRequested()
    {
        StartSingleDelivery();
    }

    //StartSingleDelivery->SetNewDelivery->StartSingleDelivery->SetNewDelivery->StartSingleDelivery->SetNewDelivery=>StartToDeliver->StartToDeliver->StartToDeliver
    /// <summary>
    /// Player teslimata ba�lar. Rota ba�lang�c� i�in hedef al�nmayacak herhangi bir bina tipini belirlemek istemiyorsan�z null y�kleyebilirsiniz.
    /// </summary>
    /// <param name="typeToAvoid"></param>
    public void StartSingleDelivery()
    {
        
        playerController.SetNewDelivery(deliveryDestinationManager.GetDeliveryDestination(), uiManager);  

    }

    public void DeliveryCompleted(Type endBuildingType)
    {
        Debug.LogWarning("Will avoid this type in the next delivery => "+ endBuildingType);
        deliveryDestinationManager.UpdateTypeToAvoid(endBuildingType);
        uiManager.DeliveryCompleted();
    }


    IEnumerator DoSomething()
    {
        yield return new WaitForSeconds(3);
        StartSingleDelivery();
    }
}
