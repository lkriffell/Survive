using UnityEngine;
using System.Linq;

public class FindBuildSite : IState
{
    private readonly Laborer _laborer;
    public bool isFound;

    public FindBuildSite(Laborer laborer) 
    {
      _laborer = laborer;
    }
    public void OnEnter() 
    {
      _laborer._lastSearch = 0f;
      isFound = false;
      BuildSite buildSite = FindActiveBuildSite();
      if (buildSite != null) 
      {
        _laborer.BuildSiteTarget = buildSite;
        isFound = true;
      }
    }
    public void Tick() 
    { 
    }

    public void OnExit() 
    {
      _laborer._lastSearch = 0f;
    }

    private BuildSite FindActiveBuildSite()
    {
      return Object.FindObjectsOfType<BuildSite>()
             .OrderBy(t=> Vector3.Distance(_laborer.transform.position, t.transform.position))
             .Where(t=> t.materialsDelivered == false)
             .Take(1)
             .FirstOrDefault();
    }
}