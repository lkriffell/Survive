using UnityEngine;
using System.Linq;

public class FindStorageToTakeFrom : IState
{
    private readonly Laborer _laborer;
    public bool isFound;

    public FindStorageToTakeFrom(Laborer laborer) 
    {
      _laborer = laborer;
    }
    public void OnEnter() 
    {
      isFound = false;
      _laborer.takeFromStorage = true;
      
      foreach(int num in Enumerable.Range(0, _laborer.BuildSiteTarget._buildPrerequisites.Count))
      {
        PickResourceToTake(num);
      // Check if build site still needs this resource
        if (_laborer.BuildSiteTarget._inventory.ContainsKey(_laborer._resourceToTake) && _laborer.BuildSiteTarget._inventory[_laborer._resourceToTake] < _laborer.BuildSiteTarget._buildPrerequisites[_laborer._resourceToTake])
        {
          break;
        }
      }

      Storage storage = FindCorrectStorage();
      if (storage != null) 
      {
        _laborer.StorageTarget = storage;
        isFound = true;
      }
    }
    public void Tick() 
    { 
    }

    public void OnExit() 
    {
    }

    private void PickResourceToTake(int index)
    {
      _laborer._resourceToTake = _laborer.BuildSiteTarget._buildPrerequisites.Keys.ElementAt(index);
      if (_laborer.BuildSiteTarget._buildPrerequisites.Values.ElementAt(index) >= _laborer.Citizen.maxCarry) _laborer._amountToTake = _laborer.Citizen.maxCarry;
      else _laborer._amountToTake = _laborer.BuildSiteTarget._buildPrerequisites.Values.ElementAt(index);
    }
    private Storage FindCorrectStorage()
    {
      return Object.FindObjectsOfType<Storage>()
             .OrderBy(t=> Vector3.Distance(_laborer.transform.position, t.transform.position))
             .Where(t=> t.acceptedResources.Contains(_laborer._resourceToTake) && t._inventory[_laborer._resourceToTake] > 0)
             .Take(1)
             .FirstOrDefault();
    }
}