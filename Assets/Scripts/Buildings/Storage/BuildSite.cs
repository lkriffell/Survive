using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildSite : MonoBehaviour
{
    public GameObject building;
    public bool readyToBuild = false;
    public bool materialsDelivered = false;
    private float _lastBuild = 0f;
    private int buildProgress = 0;
    public Dictionary<String, int> _inventory = new Dictionary<String, int>();
    public Dictionary<String, int> _buildPrerequisites = new Dictionary<String, int>(){{"Wood", 30}, {"Stone", 10}};

    void Start () 
    {
      foreach(KeyValuePair<string,int> pair in _buildPrerequisites)
      {
        _inventory.Add(pair.Key, 0);
      }
    }

    void Update ()
    {
      _lastBuild += Time.deltaTime;
      if (_inventory == _buildPrerequisites && _lastBuild > 30f) readyToBuild = true;
    }

    public void Build()
    {
      _lastBuild = 0f;
      buildProgress += 10;
      readyToBuild = false;
    }

    public bool Give(String resourceType) 
    {      
      if (InventoryEqualsPrerequisites()) 
      {
        readyToBuild = true;
        materialsDelivered = true;
        return false;
      }

      if (_inventory[resourceType] >= _buildPrerequisites[resourceType]) return false;

      _inventory[resourceType]++;

      return true;
    }

    public bool InventoryEqualsPrerequisites()
    {
      foreach (var pair in _inventory) 
      {
        int value;
        if (_buildPrerequisites.TryGetValue(pair.Key, out value)) {
          if (value != pair.Value) {
              return false;
          }
        } else {
          return false;
        }
      }
      return true;
    }
}
