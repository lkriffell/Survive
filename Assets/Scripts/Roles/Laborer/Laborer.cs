using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Laborer : MonoBehaviour
{
    private StateMachine _stateMachine;

    private float _maxCarry = 20;
    public float _carryingNow;
    public string _resourceToDeliver = "";
    public Dictionary<String, int> _inventory = new Dictionary<String, int>();

    public float _lastSearch = 0f;

    public GatherableResource ResourceTarget;
    public Storage StorageTarget;

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

      At(Wander, FindMarkedResource, StuckForOverASecond());
        Func<bool> StuckForOverASecond() => () => Wander.TimeStuck >= 1;

      At(FindMarkedResource, Wander, InventoryEmptyAndNoResourceFound());
        Func<bool> InventoryEmptyAndNoResourceFound() => () => !FindMarkedResource.isFound && _carryingNow <= 0;
      
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
        Func<bool> NextToStorageTarget() => () => StorageTarget != null && Vector3.Distance(transform.position, StorageTarget.transform.position) < 3f;

      At(GiveToStorage, FindStorage, InventoryNotEmptyAndIPlacedMyMostResourceOrStorageIsFull());
        Func<bool> InventoryNotEmptyAndIPlacedMyMostResourceOrStorageIsFull() => () => StorageTarget != null && _resourceToDeliver != null && 
          (_inventory[_resourceToDeliver] <= 0 || StorageTarget.totalStored >= StorageTarget.storable) && _carryingNow > 0;

      At(GiveToStorage, FindMarkedResource, InventoryEmpty());
        Func<bool> InventoryEmpty() => () => _carryingNow <= 0;

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

    // public void TakeFromStorage(String resourceType)
    // {
    //   if (StorageTarget.Give(resourceType))
    //   {
    //     if (_inventory.ContainsKey(resourceType)) _inventory[resourceType]++;
    //     else _inventory.Add(resourceType, 1);
    //     _carryingNow++;
    //   }
    //   else StorageTarget = null;
    // }

    public void GiveToStorage(String resourceType)
    {
      if (_inventory[resourceType] > 0 && StorageTarget.Give(resourceType))
      {
        _inventory[resourceType]--;
        _carryingNow--;
      }
      else StorageTarget = null;
    }
}
