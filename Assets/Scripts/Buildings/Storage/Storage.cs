using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public int storable = 100;

    public int totalStored = 0;

    public Dictionary<String, int> _inventory = new Dictionary<String, int>();
    public String[] acceptedResources = new String[] {"Wood", "Stone", "Metal", "Firewood"};

    void Start () 
    {
      foreach(String resource in acceptedResources)
      {
        _inventory.Add(resource, 0);
      }
    }

    void Update ()
    {

    }

    public bool Give(String resourceType) 
    {
      if (totalStored >= storable || !_inventory.ContainsKey(resourceType)) return false;
      
      _inventory[resourceType]++;
      totalStored++;
      return true;
    }

    public bool Take(String resourceType) 
    {
      if (!_inventory.ContainsKey(resourceType) || _inventory[resourceType] <= 0) return false;

      _inventory[resourceType]--;
      totalStored--;
      return true;
    }
}
