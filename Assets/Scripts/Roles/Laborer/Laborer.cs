using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Laborer : MonoBehaviour
{
    public bool takeFromStorage;
    public bool giveToStorage;
    private StateMachine _stateMachine;

    public int _maxCarry = 20;
    public float _carryingNow;
    public float _lastSearch = 0f;
    public string _resourceToDeliver = "";
    public string _resourceToTake = "";
    public int _amountToTake;
    public Dictionary<String, int> _inventory = new Dictionary<String, int>(){{"Wood", 0}, {"Stone", 0}};
    public GatherableResource ResourceTarget;
    public Storage StorageTarget;
    public BuildSite BuildSiteTarget;

    void Awake () {
      var navMeshAgent = GetComponent<NavMeshAgent>();

      _stateMachine = new StateMachine();

      var Wander = new Wander(this, navMeshAgent);

      // Resource Gathering
      var FindMarkedResource = new FindMarkedResource(this);
      var TakeFromResource = new TakeFromResource(this);
      var GoToMarkedResource = new GoToMarkedResource(this, navMeshAgent);

      // Storage 
      var FindStorage = new FindStorage(this);
      var GoToStorage = new GoToStorage(this, navMeshAgent);
      var GiveToStorage = new GiveToStorage(this);

      // BuildSite
      var FindBuildSite = new FindBuildSite(this);
      var FindStorageToTakeFrom = new FindStorageToTakeFrom(this);
      var TakeFromStorage = new TakeFromStorage(this);
      var GoToBuildSite = new GoToBuildSite(this, navMeshAgent);
      var GiveToBuildSite = new GiveToBuildSite(this);

      // Wander transitions
      At(Wander, FindMarkedResource, StuckForOverASecond());
        Func<bool> StuckForOverASecond() => () => Wander.TimeStuck >= 1;

      At(FindMarkedResource, Wander, InventoryEmptyAndNoResourceFound());
        Func<bool> InventoryEmptyAndNoResourceFound() => () => !FindMarkedResource.isFound && _carryingNow <= 0;

      At(Wander, FindBuildSite, NoResourceTargetFound());
        Func<bool> NoResourceTargetFound() => () => !FindMarkedResource.isFound && _lastSearch > 10f;

      At(FindBuildSite, Wander, InventoryEmptyAndNoBuildSite());
        Func<bool> InventoryEmptyAndNoBuildSite() => () => !FindBuildSite.isFound && _carryingNow <= 0;
      
      // Resource
      At(FindMarkedResource, GoToMarkedResource, HasResourceTargetAndInventorySpace());
        Func<bool> HasResourceTargetAndInventorySpace() => () => FindMarkedResource.isFound && _carryingNow / _maxCarry < .8f;

      At(GoToMarkedResource, FindMarkedResource, MyResourcePickedUp());
        Func<bool> MyResourcePickedUp() => () => ResourceTarget == null || (ResourceTarget != null && ResourceTarget.enabled == false);

      At(GoToMarkedResource, TakeFromResource, NextToResourceTarget());
        Func<bool> NextToResourceTarget() => () => ResourceTarget != null && Vector3.Distance(transform.position, ResourceTarget.transform.position) < 2f;

      At(TakeFromResource, FindMarkedResource, NoResourceTargetAndHasInventorySpace());
        Func<bool> NoResourceTargetAndHasInventorySpace() => () => (ResourceTarget == null || ResourceTarget != null && ResourceTarget.enabled == false) && 
        _carryingNow / _maxCarry <= .8f;

      // Storage
      At(FindMarkedResource, FindStorage, InventoryNotEmptyAndNoResourceFound());
        Func<bool> InventoryNotEmptyAndNoResourceFound() => () => (ResourceTarget == null || ResourceTarget != null && ResourceTarget.enabled == false) && 
          _carryingNow > 0f && !FindMarkedResource.isFound;

      At(TakeFromResource, FindStorage, InventoryNotEmptyAndNoResourceTarget());
        Func<bool> InventoryNotEmptyAndNoResourceTarget() => () => ((ResourceTarget == null || ResourceTarget != null && ResourceTarget.enabled == false) && 
          _carryingNow > 0f) || _carryingNow >= _maxCarry;

      At(FindStorage, Wander, NoStorageFound());
        Func<bool> NoStorageFound() => () => !FindStorage.isFound;

      At(FindStorage, GoToStorage, HasStorageTargetAndInventoryAndNoResourceFound());
        Func<bool> HasStorageTargetAndInventoryAndNoResourceFound() => () => (ResourceTarget == null || ResourceTarget != null && ResourceTarget.enabled == false) && 
          StorageTarget != null && FindStorage.isFound && _carryingNow > 0f;
      
      At(GoToStorage, GiveToStorage, NextToStorageTarget());
        Func<bool> NextToStorageTarget() => () => StorageTarget != null && Vector3.Distance(transform.position, StorageTarget.transform.position) < 3f && giveToStorage;

      At(GiveToStorage, FindStorage, InventoryNotEmptyAndIPlacedMyMostResourceOrStorageIsFull());
        Func<bool> InventoryNotEmptyAndIPlacedMyMostResourceOrStorageIsFull() => () => StorageTarget != null && _resourceToDeliver != null && 
          (_inventory[_resourceToDeliver] <= 0 || StorageTarget.totalStored >= StorageTarget.storable) && _carryingNow > 0;

      At(GiveToStorage, FindMarkedResource, InventoryEmpty());
        Func<bool> InventoryEmpty() => () => _carryingNow <= 0;

      // Build Site
      At(FindBuildSite, FindStorageToTakeFrom, HasUnfulfilledBuildSiteTarget());
        Func<bool> HasUnfulfilledBuildSiteTarget() => () => BuildSiteTarget != null && BuildSiteTarget.materialsDelivered == false;

      At(FindStorageToTakeFrom, GoToStorage, HasStorageTargetAndBuildSite());
        Func<bool> HasStorageTargetAndBuildSite() => () => BuildSiteTarget != null && StorageTarget != null;

      At(GoToStorage, TakeFromStorage, HasStorageTargetAndBuildSiteAndTakeIsTrueAndNextToStorage());
        Func<bool> HasStorageTargetAndBuildSiteAndTakeIsTrueAndNextToStorage() => () => BuildSiteTarget != null && 
          BuildSiteTarget.materialsDelivered == false && StorageTarget != null && takeFromStorage &&
            Vector3.Distance(transform.position, StorageTarget.transform.position) < 3f;

      At(TakeFromStorage, GoToBuildSite, HasBuildSiteAndEnoughResources());
        Func<bool> HasBuildSiteAndEnoughResources() => () => BuildSiteTarget != null && 
          _inventory[_resourceToTake] >= _amountToTake;

      At(GoToBuildSite, GiveToBuildSite, NextToBuildSite());
        Func<bool> NextToBuildSite() => () => BuildSiteTarget != null && Vector3.Distance(transform.position, BuildSiteTarget.transform.position) < 3f;

      At(GiveToBuildSite, FindMarkedResource, NoBuildSite());
        Func<bool> NoBuildSite() => () => BuildSiteTarget == null;

      _stateMachine.SetState(FindMarkedResource);

      void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
    }

    void FixedUpdate () 
    {
      _lastSearch += Time.deltaTime;
      _stateMachine.Tick();
    }

    public void SetMostResource()
    {
      _resourceToDeliver = _inventory.FirstOrDefault(x => x.Value == _inventory.Values.Max()).Key;
    }

    public bool InventoryFull()
    {
      if (_carryingNow >= _maxCarry) return true;
      else return false;
    }

    public void TakeFromResource()
    {
      if (!InventoryFull() && ResourceTarget.Take())
      {
        if (_inventory.ContainsKey(ResourceTarget.resourceType)) _inventory[ResourceTarget.resourceType]++;
        else _inventory.Add(ResourceTarget.resourceType, 1);
        _carryingNow++;
      }
      else ResourceTarget = null;
    }

    public void TakeFromStorage()
    {
      if (StorageTarget.Take(_resourceToTake))
      {
        if (_inventory.ContainsKey(_resourceToTake)) _inventory[_resourceToTake]++;
        else _inventory.Add(_resourceToTake, 1);
        _carryingNow++;
      }
      else StorageTarget = null;
    }

    public void GiveToStorage(String resourceType)
    {
      if (_inventory[resourceType] > 0 && StorageTarget.Give(resourceType))
      {
        _inventory[resourceType]--;
        _carryingNow--;
      }
      else StorageTarget = null;
    }

    public void GiveToBuildSite()
    {
      var give = BuildSiteTarget.Give(_resourceToTake);
      // Debug.Log(give);
      if (_inventory[_resourceToTake] > 0 && give)
      {
        _inventory[_resourceToTake]--;
        _carryingNow--;
      }
      else BuildSiteTarget = null;
    }
}
