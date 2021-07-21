using UnityEngine;
using System.Linq;

public class FindBuildSite : IState
{
    private readonly Citizen _citizen;
    public bool isFound;

    public FindBuildSite(Citizen citizen) 
    {
      _citizen = citizen;
    }
    public void OnEnter() 
    {
      _citizen._lastSearch = 0f;
      isFound = false;
      BuildSite buildSite = FindActiveBuildSite();
      if (buildSite != null) 
      {
        _citizen.BuildSiteTarget = buildSite;
        isFound = true;
      }
    }
    public void Tick() 
    { 
    }

    public void OnExit() 
    {
      _citizen._lastSearch = 0f;
    }

    private BuildSite FindActiveBuildSite()
    {
      return Object.FindObjectsOfType<BuildSite>()
             .OrderBy(t=> Vector3.Distance(_citizen.transform.position, t.transform.position))
             .Where(t=> t.materialsDelivered == false)
             .Take(1)
             .FirstOrDefault();
    }
}