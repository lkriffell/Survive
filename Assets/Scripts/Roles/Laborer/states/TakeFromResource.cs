using UnityEngine;

public class TakeFromResource : IState
{
    private readonly Citizen _citizen;

    public TakeFromResource(Citizen citizen) 
    {
      _citizen = citizen;
    }
    public void OnEnter() 
    {

    }
    public void Tick() 
    { 
      if (_citizen.ResourceTarget != null)
      {
        _citizen.TakeFromResource();
        _citizen.SetMostResource();
      }
    }

    public void OnExit() 
    {
      _citizen.SetMostResource();
      _citizen.ResourceTarget = null;
    }
}