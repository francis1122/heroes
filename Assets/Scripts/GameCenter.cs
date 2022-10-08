using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameObjects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameCenter : MonoBehaviour {

  //  private int playerFoods = 20;
 //   private int playerMinerals = 24;
    //private int playerLumber = 20;
  //  private int playerAmmo = 0;
    //public ResourceObject playerResource;
    //public GameObject mineralText;
    //public GameObject healthText;
   // public GameObject buddyText;

    public static GameCenter instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    //public ObjectFactory objectFactory;

    //public AStarGrid _aStarGrid;

    //private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
    public ResourceOrganizer resourceOrganizer;
    
    public ResourceBundle playerBufferResources;
    //public ResourceBundle playerFatiguedPopulation;
    public ResourceBundle playerResources;
    public ResourceBundle playerMaxResourceAmounts;
    public ResourceBundle playerMinResourceAmounts;

    public List<BuildingObject> purchasableBuildings = new();
    public List<BuildingObject> buildingsOwned = new();
    
    [SerializeField]
    public int currentTurn = 1;

    public int seasonsInAYear = 4;
    
    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        //    boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        resourceOrganizer = new ResourceOrganizer(Resources.LoadAll<ResourceType>("ResourceData"),
            Resources.LoadAll<PopulationType>("ResourceData/Population"));
        
        //playerResources = Instantiate(playerResources);

        
        /*
        var temp = new List<BuildingData>();
        foreach (var purchasableBuilding in purchasableBuildings)
        {
            temp.Add(Instantiate(purchasableBuilding));
        }
        purchasableBuildings.Clear();
        purchasableBuildings = temp;
        */
        
        
        
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        //  boardScript.SetupScene(level);

    }

    // Use this for initialization
    void Start()
    {
        //_aStarGrid = GetComponent<AStarGrid>();
//        ResourceObject newInstance = Instantiate(playerResource);
     //   playerResource = newInstance;
    }

    // Update is called once per frame
    void Update() {

        //after 20 seconds, reduce resources
        
    }

    private void ManagePopulation()
    {
        
        /*
        ResourceData playerPopulation = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Villager);
        ResourceData soldier = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Soldier);
        ResourceData playerFood = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Food);
        */

        int totalPopulation = GameCenter.instance.playerResources.populations
            .Sum(e => e.amount);
        ResourceData playerFood = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Food);

       // var totalPopulation = soldier.amount + playerPopulation.amount;
        if (playerFood.amount < totalPopulation)
        {
            /*
            double playerAuthF = Math.Abs(playerAuthority.amount);

            int loss = (int)Math.Ceiling((playerAuthF / 10.0f));
            
            
            
            GameCenter.instance.playerResources.SubtractResourceData(new ResourceData( loss, stabilityType));
            //ResourceData playerStabilityType = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(stabilityType);
            */
                
        }
        else
        {
            GameCenter.instance.playerBufferResources.SubtractResourceData(resourceOrganizer.CreateResourceData(totalPopulation, ResourceType.LinkType.Food));
            //GameCenter.instance.playerResources.AddResourceData(resourceOrganizer.CreateResourceData(totalPopulation, ResourceType.LinkType.Gold));
        }

    }

    private void PopulationGrowth()
    {
        ResourceData maxPop = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MaxPopulation);
        //List<ResourceData> pops = playerResources.GetMatchingResourceCategory(ResourceType.ResourceCategory.People);
        int popsTotal = playerResources.populations.Sum(e => e.amount);
        ResourceData playerFood = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Food);
        if (playerFood.amount >= popsTotal && popsTotal < maxPop.amount )
        {
            GameCenter.instance.playerBufferResources.SubtractResourceData(resourceOrganizer.CreateResourceData(popsTotal, ResourceType.LinkType.Food));
            GameCenter.instance.playerBufferResources.AddPopulationData(resourceOrganizer.CreatePopulationData(1, PopulationType.LinkPopulationType.Villager));
        }
    }

    public void EndOfTurnResourceBuffering()
    {
        //
        // Trigger resource affects first
        //
        playerResources.EndOfTurnTriggers();

        ManagePopulation();
        
        //
        // Building end of turn and year triggers
        //
        foreach (var buildingObject in buildingsOwned)
        {
            foreach (var trigger in buildingObject.buildingData.onTurnEndTrigger)
            {
                trigger.Trigger();
            }

            if (currentTurn % seasonsInAYear == 0)
            {
                foreach (var trigger in buildingObject.buildingData.onYearEndTrigger)
                {
                    Debug.Log("END OF YEAR");
                    trigger.Trigger();
                }
            }
        }
        
        // might want this to increase buffer stuff
        //EventManager.TriggerEvent(EventManager.EVENT_END_TURN);
        
        if (currentTurn % seasonsInAYear == 0)
        {
            PopulationGrowth();

            foreach (var playerResourcesPopulation in playerResources.populations)
            {
                playerResourcesPopulation.ResetActivePopulation();
            }
            //EventManager.TriggerEvent(EventManager.EVENT_END_YEAR);
        }
    }
    
    public void EndTurn()
    {
        //reset buffer
        playerBufferResources.ClearResources();

        
        EndOfTurnResourceBuffering();
        
        // push buffer
        // resource clean up
        //
        playerResources.AddResourceBundle(playerBufferResources);
        playerBufferResources.ClearResources();

        if (currentTurn % seasonsInAYear == 0)
        {
            //PopulationGrowth();
            EventManager.TriggerEvent(EventManager.EVENT_END_YEAR);
        }
        
        currentTurn += 1;
        // recalculate buffer
        EventManager.TriggerEvent(EventManager.EVENT_END_TURN);

        EndOfTurnResourceBuffering();
        EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
    }
}
