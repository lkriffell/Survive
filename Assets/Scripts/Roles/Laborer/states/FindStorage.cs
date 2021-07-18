using UnityEngine;
using System.Linq;

public class FindStorage : IState
{
    private readonly Laborer _laborer;
    public bool isFound;

    public FindStorage(Laborer laborer) 
    {
      _laborer = laborer;
    }
    public void OnEnter() 
    {
      isFound = false;
      _laborer.SetMostResource();
      // if (_laborer._resourceToDeliver == null) _laborer._resourceToDeliver = "Wood";
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

    private Storage FindCorrectStorage()
    {
      return Object.FindObjectsOfType<Storage>()
             .OrderBy(t=> Vector3.Distance(_laborer.transform.position, t.transform.position))
             .Where(t=> t.acceptedResources.Contains(_laborer._resourceToDeliver) && t.totalStored < t.storable)
             .Take(1)
             .FirstOrDefault();
    }
}