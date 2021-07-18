using UnityEngine;
using System.Linq;

public class FindMarkedResource : IState
{
    private readonly Laborer _laborer;
    public bool isFound;

    public FindMarkedResource(Laborer laborer) 
    {
      _laborer = laborer;
    }
    public void OnEnter() 
    {
      isFound = false;
      _laborer._lastSearch = 0f;
      GatherableResource resource = PickFromResourceType(5);
      if (resource == null) resource = PickFromNearest(5);
      if (resource != null) 
      {
        _laborer.ResourceTarget = resource;
        isFound = true;
      }
    }
    public void Tick() 
    { 
    }

    public void OnExit() 
    {
    }

    private GatherableResource PickFromNearest(int pickSize)
    {
      return Object.FindObjectsOfType<GatherableResource>()
             .OrderBy(t=> Vector3.Distance(_laborer.transform.position, t.transform.position))
             .Where(t=> t.markedForPickup == true && t.pickedUp == false)
             .Take(pickSize)
             .OrderBy(t => Random.Range(0, pickSize - 1))
             .FirstOrDefault();
    }

    private GatherableResource PickFromResourceType(int pickSize)
    {
      return Object.FindObjectsOfType<GatherableResource>()
             .OrderBy(t=> Vector3.Distance(_laborer.transform.position, t.transform.position))
             .Where(t=> t.markedForPickup == true && t.pickedUp == false && t.resourceType == _laborer._resourceToDeliver)
             .Take(pickSize)
             .OrderBy(t => Random.Range(0, pickSize - 1))
             .FirstOrDefault();
    }
}