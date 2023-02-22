using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameCenter : MonoBehaviour
{
    //  private int playerFoods = 20;
    //   private int playerMinerals = 24;
    //private int playerLumber = 20;
    //  private int playerAmmo = 0;
    //public ResourceObject playerResource;
    //public GameObject mineralText;
    //public GameObject healthText;
    // public GameObject buddyText;

    public static GameCenter
        instance = null; //Static instance of GameManager which allows it to be accessed by any other script.


    public GameEventManager gameEventManager;

    //public ObjectFactory objectFactory;

    //public AStarGrid _aStarGrid;

    //private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
    public ResourceOrganizer resourceOrganizer;

    public ResourceBundle playerBufferResources;

    //public ResourceBundle playerFatiguedPopulation;
    public ResourceBundle playerResources;
    public ResourceBundle playerMaxResourceAmounts;
    public ResourceBundle playerMinResourceAmounts;


    public int prestigeScore = 0; // current run prestige
    public int totalPrestigeScore = 0;
    
    public List<BuildingObject> playerBuildings = new();
    //public List<BuildingObject> buildingsOwned = new();


    //
    // Buffs/Debuffs
    //
    [SerializeField] public List<ResourceStatusEffects> endOfTurnStatusEffectsList = new();

    [SerializeField] public int currentTurn = 0;

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
        gameEventManager = GetComponent<GameEventManager>();
        resourceOrganizer = new ResourceOrganizer(Resources.LoadAll<ResourceType>("ResourceData"),
            Resources.LoadAll<PopulationType>("ResourceData/Population"));
        EventManager.StartListening(EventManager.BUILDING_CHANGED, RefreshEndOfTurnBuffer);
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
    void Update()
    {
        //after 20 seconds, reduce resources
    }


    private void RefreshEndOfTurnBuffer()
    {
        //reset buffer
        playerBufferResources.ClearResources();

        EndOfTurnResourceBuffering();

        EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
    }

//     
    private void EvaluateStability()
    {
        // negative Happiness means we lose stability
        ResourceData happiness =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Happiness);
        ResourceData happinessBuffer =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType
                .Happiness);
        if ((happinessBuffer.amount + happiness.amount) < 0)
        {
            int loss = (int)Math.Ceiling((Math.Abs(happiness.amount + happinessBuffer.amount) / 10.0f));

            instance.playerBufferResources.SubtractResourceData(new ResourceData(loss,
                resourceOrganizer.GetResourceType(ResourceType.LinkType.Stability)));
            //ResourceData playerStabilityType = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(stabilityType);
        }
    }

    private void ManagePopulation()
    {
        ResourceData playerPopulation =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType
                .MaxPopulation);
        //ResourceData soldier = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Soldier);


        int playerFood = GameCenter.instance.playerResources
            .GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Food).amount;

        int totalPopulation =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(
                ResourceType.LinkType.MaxPopulation).amount;


        ResourceData happiness =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Happiness);


        int bufferFood = GameCenter.instance.playerBufferResources
            .GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Food).amount;

        GameCenter.instance.playerBufferResources.AddResourceData(
            resourceOrganizer.CreateResourceData(totalPopulation, ResourceType.LinkType.Gold));
        GameCenter.instance.playerBufferResources.SubtractResourceData(
            resourceOrganizer.CreateResourceData(totalPopulation, ResourceType.LinkType.Food));


        if ((playerFood + bufferFood) < totalPopulation)
        {
            int lackingFood = totalPopulation - (playerFood + bufferFood);
            int loss = (int)Math.Ceiling((lackingFood / 10.0f));

            GameCenter.instance.playerBufferResources.SubtractResourceData(new ResourceData(loss,
                resourceOrganizer.GetResourceType(ResourceType.LinkType.Happiness)));
            //ResourceData playerStabilityType = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(stabilityType);
        }
    }

    // private void PopulationGrowth()
    // {
    //     ResourceData maxPop = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MaxPopulation);
    //     //List<ResourceData> pops = playerResources.GetMatchingResourceCategory(ResourceType.ResourceCategory.People);
    //     int popsTotal = playerResources.populations.Sum(e => e.amount);
    //     ResourceData playerFood = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Food);
    //     if (playerFood.amount >= popsTotal && popsTotal < maxPop.amount )
    //     {
    //         GameCenter.instance.playerBufferResources.SubtractResourceData(resourceOrganizer.CreateResourceData(popsTotal, ResourceType.LinkType.Food));
    //         GameCenter.instance.playerBufferResources.AddPopulationData(resourceOrganizer.CreatePopulationData(1, PopulationType.LinkPopulationType.Villager));
    //     }
    // }

    public void ChangePlayerResourcesEndOfTurn(ResourceBundle changeBundle, StatusIdentifier statusIdentifier)
    {
        var useBundle = changeBundle;
        // check if there are buffs to do
        if (statusIdentifier != null)
        {
            foreach (var resourceStatusEffects in endOfTurnStatusEffectsList)
            {
                if (statusIdentifier.nameList.Intersect(resourceStatusEffects.statusIdentifier.nameList).Any())
                {
                    useBundle = new ResourceBundle(useBundle, resourceStatusEffects);
                }
            }
        }

        playerBufferResources.AddResourceBundle(useBundle);
    }

    public bool CanChangePlayerResources(ResourceBundle changeBundle, bool canPartiallyAdd,
        StatusIdentifier statusIdentifier)
    {
        return playerResources.CanAddResourceBundle(changeBundle, canPartiallyAdd);
    }

    public void ChangePlayerResources(ResourceBundle changeBundle, StatusIdentifier statusIdentifier)
    {
        playerResources.AddResourceBundle(changeBundle);
    }


    public void EndOfTurnResourceBuffering()
    {
        //
        // Trigger resource affects first
        //
        playerResources.EndOfTurnTriggers();


        //
        // Building end of turn and year triggers
        //
        foreach (var buildingObject in playerBuildings)
        {
            for (int i = 0; i < buildingObject.buildingsOwned; i++)
            {
                foreach (var trigger in buildingObject.buildingData.onTurnEndTrigger)
                {
                    //trigger.Trigger();
                    trigger.Trigger(new StatusIdentifier(new List<String> { buildingObject.buildingData.uniqueName }));
                }

                if ((currentTurn + 1) % seasonsInAYear == 0)
                {
                    foreach (var trigger in buildingObject.buildingData.onYearEndTrigger)
                    {
                        Debug.Log("END OF YEAR");
                        trigger.Trigger(
                            new StatusIdentifier(new List<String> { buildingObject.buildingData.uniqueName }));
                    }
                }
            }
        }

        foreach (var resourceType in playerResources.resources)
        {
            foreach (var trigger in resourceType.type.playerEndOfTurnTriggers)
            {
                //trigger.Trigger();
                trigger.Trigger(new StatusIdentifier(new List<String> { resourceType.getShortString() }));
            }
        }


        ManagePopulation();
        EvaluateStability(); // if player starts turn with an unhappiness score, loss stability
        // might want this to increase buffer stuff
        //EventManager.TriggerEvent(EventManager.EVENT_END_TURN);

        /*if (currentTurn % seasonsInAYear == 0)
        {
            //PopulationGrowth();

            foreach (var playerResourcesPopulation in playerResources.populations)
            {
                playerResourcesPopulation.ResetActivePopulation();
                playerResourcesPopulation.annualRecruitsAvailable = playerResourcesPopulation.annualRecruitLimit;
            }
            
            //EventManager.TriggerEvent(EventManager.EVENT_END_YEAR);
        }*/
    }

    public void EndTurn()
    {
        //reset buffer
        playerBufferResources.ClearResources();


        EndOfTurnResourceBuffering();


        // clear buildings time purchased Stat
        for (int i = playerBuildings.Count - 1; i >= 0; i--)
        {
            var buildingObject = playerBuildings[i];
            buildingObject.timesPurchasedThisTurn = 0;
            if (buildingObject.buildingData.category == BuildingData.BuildingCategory.Event)
            {
                buildingObject.eventLifeSpanLeft--;
                if (buildingObject.eventLifeSpanLeft <= 0)
                {
                    foreach (var trigger in buildingObject.buildingData.onExpiredEvent)
                    {
                        //trigger.Trigger();
                        trigger.Trigger(new StatusIdentifier(new List<String>
                            { buildingObject.buildingData.buildingName }));
                    }

                    playerBuildings.RemoveAt(i);
                }
            }
        }


        // push buffer
        // resource clean up
        //
        playerResources.AddResourceBundle(playerBufferResources);
        playerBufferResources.ClearResources();

        //
        // Start of turn
        //


        if ((currentTurn + 1) % seasonsInAYear == 0)
        {
            // clear buildings time purchased Stat
            foreach (var buildingObject in playerBuildings)
            {
                buildingObject.timesPurchasedThisYear = 0;
            }

            EventManager.TriggerEvent(EventManager.EVENT_END_YEAR);
        }

        currentTurn += 1;
        prestigeScore++;

        // Evaluate if player has lost
        if (playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Stability).amount <= 0)
        {
            SceneManager.LoadScene("EndGameNewGameScene");
        }

        //Start of New Turn
        gameEventManager.OnTurnStart(currentTurn, (currentTurn) % seasonsInAYear == 0);
        // recalculate buffer
        EventManager.TriggerEvent(EventManager.EVENT_END_TURN);

        EndOfTurnResourceBuffering();
        EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
    }
}