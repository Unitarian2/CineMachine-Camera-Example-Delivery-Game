using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private List<IBuilding> buildingList = new();
    [SerializeField] private Transform buildingParentObject;
    [SerializeField] private PlayerController playerController;

    DeliveryDestinationManager deliveryDestinationManager;
    // Start is called before the first frame update
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
        StartCoroutine(DoSomething());
        
    }

    private void InitDeliverySystem()
    {
        deliveryDestinationManager = new DeliveryDestinationManager(buildingList);
    }

    /// <summary>
    /// Player teslimata baþlar. Rota baþlangýcý için hedef alýnmayacak herhangi bir bina tipini belirlemek istemiyorsanýz null yükleyebilirsiniz.
    /// </summary>
    /// <param name="typeToAvoid"></param>
    public void StartSingleDelivery(Type typeToAvoid)
    {
        playerController.SetNewDelivery(deliveryDestinationManager.GetDeliveryDestination(typeToAvoid));
        playerController.StartToDeliver();
    }

    IEnumerator DoSomething()
    {
        yield return new WaitForSeconds(3);
        StartSingleDelivery(null);
    }
}
