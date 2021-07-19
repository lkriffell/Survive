using UnityEngine;

public class GiveToBuildSite : IState
{
    private readonly Laborer _laborer;

    private string _mostResource;

    public GiveToBuildSite(Laborer laborer) 
    {
      _laborer = laborer;
    }
    public void OnEnter() 
    {
    }
    public void Tick() 
    { 
      if (_laborer.BuildSiteTarget != null)
      {
        _laborer.GiveToBuildSite();
      }
    }

    public void OnExit() 
    {
      // _laborer._resourceToTake = "";
    }
}