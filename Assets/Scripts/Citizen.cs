using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Citizen : MonoBehaviour
{
    public int level = 1;
    public float health = 100;
    public int maxCarry = 20;
    public int carryingNow = 0;
    public string _resourceToDeliver = "";
    public string _resourceToTake = "";
    public int _amountToTake;

    public bool takeFromStorage;
    public bool giveToStorage;

    public Dictionary<String, int> _inventory = new Dictionary<String, int>(){{"Wood", 0}, {"Stone", 0}};

    public float _lastSearch = 0f;
    public GatherableResource ResourceTarget;
    public Storage StorageTarget;
    public BuildSite BuildSiteTarget;
    public Builder PrimaryRole;
    public Laborer SecondaryRole;

    private void Awake()
    {
        if (PrimaryRole != null) SecondaryRole.enabled = false;
    }

    void FixedUpdate() => _lastSearch += Time.deltaTime;

    public void SetMostResource()
    {
      _resourceToDeliver = _inventory.FirstOrDefault(x => x.Value == _inventory.Values.Max()).Key;
    }

    public bool InventoryFull()
    {
      if (carryingNow >= maxCarry) return true;
      else return false;
    }

    public void TakeFromResource()
    {
      if (!InventoryFull() && ResourceTarget.Take())
      {
        if (_inventory.ContainsKey(ResourceTarget.resourceType)) _inventory[ResourceTarget.resourceType]++;
        else _inventory.Add(ResourceTarget.resourceType, 1);
        carryingNow++;
      }
      else ResourceTarget = null;
    }

    public void TakeFromStorage()
    {
      if (StorageTarget.Take(_resourceToTake))
      {
        if (_inventory.ContainsKey(_resourceToTake)) _inventory[_resourceToTake]++;
        else _inventory.Add(_resourceToTake, 1);
        carryingNow++;
      }
      else StorageTarget = null;
    }

    public void GiveToStorage(String resourceType)
    {
      if (_inventory[resourceType] > 0 && StorageTarget.Give(resourceType))
      {
        _inventory[resourceType]--;
        carryingNow--;
      }
      else StorageTarget = null;
    }

    public void GiveToBuildSite()
    {
      if (_inventory[_resourceToTake] > 0 && BuildSiteTarget.Give(_resourceToTake))
      {
        _inventory[_resourceToTake]--;
        carryingNow--;
      }
      else BuildSiteTarget = null;
    }
}