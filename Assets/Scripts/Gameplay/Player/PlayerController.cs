using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    //Dependencies
    Grid grid;
    Unit playerUnit;
    GameManager gameManager;
    CineCameraManager cameraMan;

    Vector3 oldPos;
    Vector3 newPos;

    [SerializeField] private Transform returnPoint;

    private IBuilding deliveryStartBuilding;
    private IBuilding deliveryEndBuilding;

    public PlayerDeliveryType playerDeliveryState = PlayerDeliveryType.Waiting;

    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    public void InitPlayerController(GameManager gameManager, CineCameraManager cameraMan)
    {
        this.gameManager = gameManager;
        this.cameraMan = cameraMan;
        cameraMan.StartDollies();
    }
    
    public void InitPlayerMapInfo(Grid grid, Unit playerUnit)
    {
        this.grid = grid;
        this.playerUnit = playerUnit;
        
    }
    /// <summary>
    /// Gidilecek baþlangýç ve bitiþ rotasýný oluþturur. newDelivery'de verilen bina tiplerine göre seçimi yapar.
    /// </summary>
    /// <param name="newDelivery"></param>
    public void SetNewDelivery(DeliveryDestination newDelivery, UIManager uiManager)
    {
        Debug.LogWarning("Start : " + newDelivery.BuildingStart.BuildingName + " / "+ "End : "+ newDelivery.BuildingEnd.BuildingName);

        #region Start Route
        //Rotanýn baþlayacaðý bina belirleniyor. Önce yakýn civardaki binalarda arama yapýlýyor. Bulunamaz veya bir bug oluþursa tüm binalarda arama yapýlýyor.
        BuildingFinder buildingFinderStart = new(newDelivery.BuildingStart, grid.GetNearbyBuildings(gameObject.transform.position));
        IBuilding closestBuildingStart = buildingFinderStart.FindSameBuildingsByType().FindClosestBuilding(gameObject.transform.position);
        if(closestBuildingStart == null)
        {
            Debug.Log("Building Not Found, Searching All Buildings");
            BuildingFinder buildingFinderStartAll = new(newDelivery.BuildingStart, grid.GetAllBuildings());
            closestBuildingStart = buildingFinderStartAll.FindSameBuildingsByType().FindClosestBuilding(gameObject.transform.position);
        }
        #endregion

        #region End Route
        //Rotanýn biteceði bina belirleniyor. Önce yakýn civardaki binalarda arama yapýlýyor. Bulunamaz veya bir bug oluþursa tüm binalarda arama yapýlýyor.
        BuildingFinder buildingFinderEnd = new(newDelivery.BuildingEnd, grid.GetNearbyBuildings(closestBuildingStart.EntryPoint.transform.position));
        IBuilding closestBuildingEnd = buildingFinderEnd.FindSameBuildingsByType().FindClosestBuilding(closestBuildingStart.EntryPoint.transform.position);
        if (closestBuildingEnd == null)
        {
            Debug.Log("Building Not Found, Searching All Buildings");
            BuildingFinder buildingFinderEndAll = new(newDelivery.BuildingEnd, grid.GetAllBuildings());
            closestBuildingEnd = buildingFinderEndAll.FindSameBuildingsByType().FindClosestBuilding(closestBuildingStart.EntryPoint.transform.position);
        }
        #endregion

        

        //Baþlangýç ve bitiþ baþarýlý bir þekilde bulunduysa hedefleri set ediyoruz. Bu if clause bug durumuna karþýn konuldu.
        if (closestBuildingStart != null && closestBuildingEnd != null)
        {
            //Yakýnda bina bulmuþuz
            Debug.Log("Route Found : "+ closestBuildingStart.GameObject.name + " to "+ closestBuildingEnd.GameObject.name);
            deliveryStartBuilding = closestBuildingStart;
            deliveryEndBuilding = closestBuildingEnd;

            uiManager.NewDeliveryDestinationArrived(new DeliveryDestination(deliveryStartBuilding,deliveryEndBuilding));
            StartToDeliver();
        }
        else
        {
            //Yakýnda bina bulamamýþýz. Tüm haritada arayacaðýz.
            Debug.Log("Building Not Found, Searching All Buildings");
            BuildingFinder buildingFinderStartAll = new(newDelivery.BuildingStart, grid.GetAllBuildings());
            deliveryStartBuilding = buildingFinderStartAll.FindSameBuildingsByType().FindClosestBuilding(gameObject.transform.position);

            BuildingFinder buildingFinderEndAll = new(newDelivery.BuildingEnd, grid.GetAllBuildings());
            deliveryEndBuilding = buildingFinderEndAll.FindSameBuildingsByType().FindClosestBuilding(deliveryStartBuilding.EntryPoint.transform.position);

            

            if (closestBuildingStart == null || closestBuildingEnd == null)
            {
                Debug.LogError("Invalid Route, Starting a New Delivery");
                gameManager.StartSingleDelivery();
            }
            else
            {
                Debug.Log("Route Found : " + closestBuildingStart.GameObject.name + " to " + closestBuildingEnd.GameObject.name);
                uiManager.NewDeliveryDestinationArrived(new DeliveryDestination(deliveryStartBuilding, deliveryEndBuilding));
                StartToDeliver();
            }
        }
    }

    /// <summary>
    /// Player Object rotaya baþlar. Bunu çaðýrmadan önce SetNewDelivery ile rota belirleyin. Zaten bir baþlangýç noktasýna gidiyorsa, onu bitirmeden yeni rotaya gitmez.
    /// </summary>
    public void StartToDeliver()
    {   
        if(playerDeliveryState != PlayerDeliveryType.MovingToStart)
        {  
            SetDestination(deliveryStartBuilding);
            playerDeliveryState = PlayerDeliveryType.MovingToStart;
            cameraMan.StopDollyProcess();
        }
        
    }

    /// <summary>
    /// Player Object'i bir binanýn Entry Point'ine gönderir.
    /// </summary>
    /// <param name="buildingDestination"></param>
    private void SetDestination(IBuilding buildingDestination)
    {
        agent.SetDestination(buildingDestination.EntryPoint.transform.position);
    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log("Mouse clicked");
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if(Physics.Raycast(ray,out hit))
        //    {
        //        agent.SetDestination(hit.point);
        //    }
        //}


        //Player'ýn içinde bulunduðu Grid Cell deðiþiminin kontrolü.
        oldPos = newPos;
        newPos = transform.position;
        grid.CheckPlayerMovement(playerUnit,oldPos,newPos);


    }

    void OnTriggerEnter(Collider other)
    {
        if(playerDeliveryState != PlayerDeliveryType.Waiting)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("DeliveryPoint"))
            {
                //Bir Teslimat Noktasýnýn sýnýrlarýna girmiþiz.
                if (other.gameObject.transform.parent.TryGetComponent<IBuilding>(out IBuilding building))
                {
                    if (building == deliveryStartBuilding && playerDeliveryState == PlayerDeliveryType.MovingToStart)
                    {
                        Debug.Log("Start Building'e ulaþýldý!");
                        playerDeliveryState = PlayerDeliveryType.Waiting;
                        SetDestination(deliveryEndBuilding);
                        playerDeliveryState = PlayerDeliveryType.MovingToEnd;
                    }
                    else if (building == deliveryEndBuilding && playerDeliveryState == PlayerDeliveryType.MovingToEnd)
                    {
                        Debug.Log("End Building'e ulaþýldý!");
                        playerDeliveryState = PlayerDeliveryType.Waiting;
                        //agent.isStopped = true;
                        gameManager.DeliveryCompleted(deliveryEndBuilding.Type);
                        ReturnToStartCenter();
                        //gameManager.StartSingleDelivery();
                    }
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("CenterPoint"))
            {

                if (playerDeliveryState == PlayerDeliveryType.ReturningToCenter)
                {
                    //Baþlangýç Noktasýna dönmüþ
                    playerDeliveryState = PlayerDeliveryType.Waiting;
                    cameraMan.StartDollies();
                }

            }
            
            
        }
    }

    void ReturnToStartCenter()
    {
        playerDeliveryState = PlayerDeliveryType.ReturningToCenter;
        agent.SetDestination(returnPoint.position);
    }
   
}

public enum PlayerDeliveryType
{
    Waiting,
    MovingToStart,
    MovingToEnd,
    ReturningToCenter
}

