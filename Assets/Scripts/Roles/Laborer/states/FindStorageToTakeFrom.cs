using UnityEngine;
using System.Linq;

public class FindStorageToTakeFrom : IState
{
    private readonly Citizen _citizen;
    public bool isFound;

    public FindStorageToTakeFrom(Citizen citizen) 
    {
      _citizen = citizen;
    }
    public void OnEnter() 
    {
      isFound = false;
      _citizen.takeFromStorage = true;
      
      foreach(int num in Enumerable.Range(0, _citizen.BuildSiteTarget._buildPrerequisites.Count))
      {
        PickResourceToTake(num);
      // Check if build site still needs this resource
        if (_citizen.BuildSiteTarget._inventory.ContainsKey(_citizen._resourceToTake) && _citizen.BuildSiteTarget._inventory[_citizen._resourceToTake] < _citizen.BuildSiteTarget._buildPrerequisites[_citizen._resourceToTake])
        {
          break;
        }
      }

      Storage storage = FindCorrectStorage();
      if (storage != null) 
      {
        _citizen.StorageTarget = storage;
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
      _citizen._resourceToTake = _citizen.BuildSiteTarget._buildPrerequisites.Keys.ElementAt(index);
      if (_citizen.BuildSiteTarget._buildPrerequisites.Values.ElementAt(index) >= _citizen.maxCarry) _citizen._amountToTake = _citizen.maxCarry;
      else _citizen._amountToTake = _citizen.BuildSiteTarget._buildPrerequisites.Values.ElementAt(index);
    }
    private Storage FindCorrectStorage()
    {
      return Object.FindObjectsOfType<Storage>()
             .OrderBy(t=> Vector3.Distance(_citizen.transform.position, t.transform.position))
             .Where(t=> t.acceptedResources.Contains(_citizen._resourceToTake) && t._inventory[_citizen._resourceToTake] > 0)
             .Take(1)
             .FirstOrDefault();
    }
}