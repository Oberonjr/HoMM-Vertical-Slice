using System;
using UnityEngine;

public static class OverworldEventBus<T> where T : Event
{
    public static event Action<T> OnEvent;

    public static void Publish(T pEvent)
    {
        OnEvent?.Invoke(pEvent);
    }
}

//////////////////////////////////////General events
// Start
public class InitializeWorld : Event
{
    public InitializeWorld(){}
}

//Calendar
public class NewDay: Event{}

public class NewWeek: Event{}

public class NewMonth : Event{}

//UI
public class OpenTownScreen : Event
{
    public TownData town;

    public OpenTownScreen(TownData pTownData)
    {
        town = pTownData;
    }
    
}
public class OpenRecruitScreen : Event
{
    public CreatureDwellingInfo dwellingInfo;
    public UnitStats unit;
    public int amount;

    public OpenRecruitScreen(CreatureDwellingInfo pDwellingInfo)
    {
        dwellingInfo = pDwellingInfo;
        unit = pDwellingInfo.ProducedUnit;
        amount = pDwellingInfo.StationedAmont;
    }

}

public class OpenBuildScreen : Event
{
    public TownBuildingData buildingData;
    public GameObject buildingToBuild;
    public GameObject buildingToReplace;

    public OpenBuildScreen(TownBuildingData pBuildingData, GameObject pBuildingToBuild, GameObject pBuildingToReplace)
    {
        buildingData = pBuildingData;
        buildingToBuild = pBuildingToBuild;
        buildingToReplace = pBuildingToReplace;
    }
}

//////////////////////////////////////Player related events
//Turn management
public class OnPlayerTurnStart : Event
{
    public Player player;

    public OnPlayerTurnStart(Player pPlayer)
    {
        player = pPlayer;
    }
}

public class OnPlayerTurnEnd : Event
{
    public Player player;

    public OnPlayerTurnEnd(Player pPlayer)
    {
        player = pPlayer;
    }
}

///////////Economy
public class UpdateKindgomIncome : Event
{
    public Player player;

    public UpdateKindgomIncome(Player pPlayer)
    {
        player = pPlayer;
    }
}

public class RecruitUnit : Event
{
    public UnitStats unit;
    public int amount;

    public RecruitUnit(UnitStats pUnit, int pAmount)
    {
        unit = pUnit;
        amount = pAmount;
    }
}

//Town-specific
public class BuildTownBuilding : Event
{
    public TownBuildingData builtBuilding;

    public BuildTownBuilding(TownBuildingData pBuildingData)
    {
        builtBuilding = pBuildingData;
    }
}

//////////////////////////////////////Hero related events
//Movement
public class OnHeroMoveStart : Event
{
    public HeroManager hero;
    public Node startingLocation;

    public OnHeroMoveStart(HeroManager pHero, Node pNode)
    {
        hero = pHero;
        startingLocation = pNode;
    }
}

public class OnHeroMoveEnd : Event
{
    public HeroManager hero;
    public Node endingLocation;

    public OnHeroMoveEnd(HeroManager pHero, Node pNode)
    {
        hero = pHero;
        endingLocation = pNode;
    }
}

public class OnHeroMoving : Event
{
    public HeroManager hero;
    public Node currentLocation;

    public OnHeroMoving(HeroManager pHero, Node pNode)
    {
        hero = pHero;
        currentLocation = pNode;
    }
}

//Interaction
public class OnHeroInteract : Event
{
    public HeroManager hero;
    public Interactable interactable;

    public OnHeroInteract(HeroManager pHero, Interactable pInteractable)
    {
        hero = pHero;
        interactable = pInteractable;
    }
}

//Initiate Combat
public class InitiateCombat : Event
{
    public Army AggressorArmy { get; }
    public GameObject AggresorObject { get; }
    public Army DefenderArmy { get; }
    public GameObject DefenderObject { get; }

    public InitiateCombat(Army pAggressorArmy, Army pDefenderArmy, GameObject pAggressorObject, GameObject pDefenderObject)
    {
        AggressorArmy = pAggressorArmy;
        DefenderArmy = pDefenderArmy;
        AggresorObject = pAggressorObject;
        DefenderObject = pDefenderObject;
    }
}
