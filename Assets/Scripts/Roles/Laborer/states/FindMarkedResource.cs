using UnityEngine;
using System.Linq;

public class FindMarkedResource : IState
{
    private readonly Citizen _citizen;
    public bool isFound;

    public FindMarkedResource(Citizen citizen) 
    {
      _citizen = citizen;
    }
    public void OnEnter() 
    {
      isFound = false;
      GatherableResource resource = PickFromResourceType(5);
      if (resource == null) resource = PickFromNearest(5);
      if (resource != null) 
      {
        _citizen.ResourceTarget = resource;
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
             .OrderBy(t=> Vector3.Distance(_citizen.transform.position, t.transform.position))
             .Where(t=> t.markedForPickup == true && t.pickedUp == false)
             .Take(pickSize)
             .OrderBy(t => Random.Range(0, pickSize - 1))
             .FirstOrDefault();
    }

    private GatherableResource PickFromResourceType(int pickSize)
    {
      return Object.FindObjectsOfType<GatherableResource>()
             .OrderBy(t=> Vector3.Distance(_citizen.transform.position, t.transform.position))
             .Where(t=> t.markedForPickup == true && t.pickedUp == false && t.resourceType == _citizen._resourceToDeliver)
             .Take(pickSize)
             .OrderBy(t => Random.Range(0, pickSize - 1))
             .FirstOrDefault();
    }
}