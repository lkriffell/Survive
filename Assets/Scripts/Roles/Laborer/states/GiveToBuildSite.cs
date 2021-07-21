using UnityEngine;

public class GiveToBuildSite : IState
{
    private readonly Citizen _citizen;

    private string _mostResource;

    public GiveToBuildSite(Citizen citizen) 
    {
      _citizen = citizen;
    }
    public void OnEnter() 
    {
    }
    public void Tick() 
    { 
      if (_citizen.BuildSiteTarget != null)
      {
        _citizen.GiveToBuildSite();
      }
    }

    public void OnExit() 
    {
    }
}