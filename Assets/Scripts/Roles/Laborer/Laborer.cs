using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Laborer : MonoBehaviour
{
    private StateMachine _stateMachine;
    
    public Citizen Citizen;

    void Awake () {
      var navMeshAgent = Citizen.GetComponent<NavMeshAgent>();

      _stateMachine = new StateMachine();

      var Wander = new Wander(Citizen, navMeshAgent);

      // Resource Gathering
      var FindMarkedResource = new FindMarkedResource(Citizen);
      var TakeFromResource = new TakeFromResource(Citizen);
      var GoToMarkedResource = new GoToMarkedResource(Citizen, navMeshAgent);

      // Storage 
      var FindStorage = new FindStorage(Citizen);
      var GoToStorage = new GoToStorage(Citizen, navMeshAgent);
      var GiveToStorage = new GiveToStorage(Citizen);

      // BuildSite
      var FindBuildSite = new FindBuildSite(Citizen);
      var FindStorageToTakeFrom = new FindStorageToTakeFrom(Citizen);
      var TakeFromStorage = new TakeFromStorage(Citizen);
      var GoToBuildSite = new GoToBuildSite(Citizen, navMeshAgent);
      var GiveToBuildSite = new GiveToBuildSite(Citizen);

      // Wander transitions
      At(Wander, FindMarkedResource, StuckForOverASecond());
        Func<bool> StuckForOverASecond() => () => Wander.TimeStuck >= 1;

      At(FindMarkedResource, Wander, InventoryEmptyAndNoResourceFound());
        Func<bool> InventoryEmptyAndNoResourceFound() => () => !FindMarkedResource.isFound && Citizen.carryingNow <= 0;

      At(Wander, FindBuildSite, NoResourceTargetFound());
        Func<bool> NoResourceTargetFound() => () => !FindMarkedResource.isFound && Citizen._lastSearch > 10f;

      At(FindBuildSite, Wander, InventoryEmptyAndNoBuildSite());
        Func<bool> InventoryEmptyAndNoBuildSite() => () => !FindBuildSite.isFound && Citizen.carryingNow <= 0;
      
      // Resource
      At(FindMarkedResource, GoToMarkedResource, HasResourceTargetAndInventorySpace());
        Func<bool> HasResourceTargetAndInventorySpace() => () => FindMarkedResource.isFound && Citizen.carryingNow / Citizen.maxCarry < .8f;

      At(GoToMarkedResource, FindMarkedResource, MyResourcePickedUp());
        Func<bool> MyResourcePickedUp() => () => Citizen.ResourceTarget == null || (Citizen.ResourceTarget != null && Citizen.ResourceTarget.enabled == false);

      At(GoToMarkedResource, TakeFromResource, NextToResourceTarget());
        Func<bool> NextToResourceTarget() => () => Citizen.ResourceTarget != null && Vector3.Distance(transform.position, Citizen.ResourceTarget.transform.position) < 2f;

      At(TakeFromResource, FindMarkedResource, NoResourceTargetAndHasInventorySpace());
        Func<bool> NoResourceTargetAndHasInventorySpace() => () => (Citizen.ResourceTarget == null || Citizen.ResourceTarget != null && Citizen.ResourceTarget.enabled == false) && 
        Citizen.carryingNow / Citizen.maxCarry <= .8f;

      // Storage
      At(FindMarkedResource, FindStorage, InventoryNotEmptyAndNoResourceFound());
        Func<bool> InventoryNotEmptyAndNoResourceFound() => () => (Citizen.ResourceTarget == null || Citizen.ResourceTarget != null && Citizen.ResourceTarget.enabled == false) && 
          Citizen.carryingNow > 0f && !FindMarkedResource.isFound;

      At(TakeFromResource, FindStorage, InventoryNotEmptyAndNoResourceTarget());
        Func<bool> InventoryNotEmptyAndNoResourceTarget() => () => ((Citizen.ResourceTarget == null || Citizen.ResourceTarget != null && Citizen.ResourceTarget.enabled == false) && 
          Citizen.carryingNow > 0f) || Citizen.carryingNow >= Citizen.maxCarry;

      At(FindStorage, Wander, NoStorageFound());
        Func<bool> NoStorageFound() => () => !FindStorage.isFound;

      At(FindStorage, GoToStorage, HasStorageTargetAndInventoryAndNoResourceFound());
        Func<bool> HasStorageTargetAndInventoryAndNoResourceFound() => () => (Citizen.ResourceTarget == null || Citizen.ResourceTarget != null && Citizen.ResourceTarget.enabled == false) && 
          Citizen.StorageTarget != null && FindStorage.isFound && Citizen.carryingNow > 0f;
      
      At(GoToStorage, GiveToStorage, NextToStorageTarget());
        Func<bool> NextToStorageTarget() => () => Citizen.StorageTarget != null && Vector3.Distance(transform.position, Citizen.StorageTarget.transform.position) < 3f && Citizen.giveToStorage;

      At(GiveToStorage, FindStorage, InventoryNotEmptyAndIPlacedMyMostResourceOrStorageIsFull());
        Func<bool> InventoryNotEmptyAndIPlacedMyMostResourceOrStorageIsFull() => () => Citizen.StorageTarget != null && Citizen._resourceToDeliver != null && 
          (Citizen._inventory[Citizen._resourceToDeliver] <= 0 || Citizen.StorageTarget.totalStored >= Citizen.StorageTarget.storable) && Citizen.carryingNow > 0;

      At(GiveToStorage, FindMarkedResource, InventoryEmpty());
        Func<bool> InventoryEmpty() => () => Citizen.carryingNow <= 0;

      // Build Site
      At(FindBuildSite, FindStorageToTakeFrom, HasUnfulfilledBuildSiteTarget());
        Func<bool> HasUnfulfilledBuildSiteTarget() => () => Citizen.BuildSiteTarget != null && Citizen.BuildSiteTarget.materialsDelivered == false;

      At(FindStorageToTakeFrom, GoToStorage, HasStorageTargetAndBuildSite());
        Func<bool> HasStorageTargetAndBuildSite() => () => Citizen.BuildSiteTarget != null && Citizen.StorageTarget != null;

      At(GoToStorage, TakeFromStorage, HasStorageTargetAndBuildSiteAndTakeIsTrueAndNextToStorage());
        Func<bool> HasStorageTargetAndBuildSiteAndTakeIsTrueAndNextToStorage() => () => Citizen.BuildSiteTarget != null && 
          Citizen.BuildSiteTarget.materialsDelivered == false && Citizen.StorageTarget != null && Citizen.takeFromStorage &&
            Vector3.Distance(transform.position, Citizen.StorageTarget.transform.position) < 3f;

      At(TakeFromStorage, GoToBuildSite, HasBuildSiteAndEnoughResources());
        Func<bool> HasBuildSiteAndEnoughResources() => () => Citizen.BuildSiteTarget != null && 
          Citizen._inventory[Citizen._resourceToTake] >= Citizen._amountToTake;

      At(GoToBuildSite, GiveToBuildSite, NextToBuildSite());
        Func<bool> NextToBuildSite() => () => Citizen.BuildSiteTarget != null && Vector3.Distance(transform.position, Citizen.BuildSiteTarget.transform.position) < 3f;

      At(GiveToBuildSite, FindMarkedResource, NoBuildSite());
        Func<bool> NoBuildSite() => () => Citizen.BuildSiteTarget == null;

      _stateMachine.SetState(FindMarkedResource);

      void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
    }

    void FixedUpdate () 
    {
      _stateMachine.Tick();
    }
}
