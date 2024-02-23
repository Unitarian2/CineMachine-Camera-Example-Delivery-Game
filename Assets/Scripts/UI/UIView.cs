using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    

    [SerializeField] private Button newDeliveryButton;

    [SerializeField] private Image startBuildingImage;
    [SerializeField] private Image endBuildingImage;

    [SerializeField] private TextMeshProUGUI deliveryTimer;


    public event Action OnNewDeliveryTriggered;

    // Start is called before the first frame update
    void Start()
    {
        newDeliveryButton.onClick.AddListener(()=> OnNewDeliveryButtonClicked());
    }

    void OnNewDeliveryButtonClicked()
    {
        newDeliveryButton.gameObject.SetActive(false);
        ResetUI();
        OnNewDeliveryTriggered?.Invoke();
    }

    public void SetDeliveryBuildingImages(Sprite startBuilding, Sprite endBuilding)
    {
        startBuildingImage.sprite = startBuilding; endBuildingImage.sprite = endBuilding;
    }

    public void SetTimer(float timer)
    {
        deliveryTimer.text = timer.ToString("F2");
    }

    public void RestartUIButtons()
    {
        newDeliveryButton.gameObject.SetActive(true);
    }

    private void ResetUI()
    {
        startBuildingImage.sprite = null; endBuildingImage.sprite = null;
        deliveryTimer.text = "0.00";
    }

}
