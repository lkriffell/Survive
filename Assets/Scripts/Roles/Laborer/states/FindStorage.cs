using UnityEngine;
using System.Linq;

public class FindStorage : IState
{
    private readonly Citizen _citizen;
    public bool isFound;

    public FindStorage(Citizen citizen) 
    {
      _citizen = citizen;
    }
    public void OnEnter() 
    {
      isFound = false;
      _citizen.giveToStorage = true;
      _citizen.SetMostResource();
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

    private Storage FindCorrectStorage()
    {
      return Object.FindObjectsOfType<Storage>()
             .OrderBy(t=> Vector3.Distance(_citizen.transform.position, t.transform.position))
             .Where(t=> t.acceptedResources.Contains(_citizen._resourceToDeliver) && t.totalStored < t.storable)
             .Take(1)
             .FirstOrDefault();
    }
}